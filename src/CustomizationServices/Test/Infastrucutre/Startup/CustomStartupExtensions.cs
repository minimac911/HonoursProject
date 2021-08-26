using Autofac;
using Consul;
using EventBus;
using EventBus.EventBusRabbitMQ;
using EventBus.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Test.Helper;
using Test.Infastrucutre.Tenancy;
using Test.Services;

namespace Test.Infastrucutre.Startup
{
    public static class CustomStartupExtenstions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string mySqlConnectionStr = ConnectionStringHelper.GetConnectionString(configuration);
            //services.AddDbContextPool<CartContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            return services;
        }

        public static IServiceCollection AddMVCControllers(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            return services;
        }

        public static IServiceCollection AddIntegrationEvents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new RabbitMQPersistentConnection(factory, logger, retryCount);
            });


            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = configuration["SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, iLifetimeScope, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            // Add Event Handlers
            //services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                var ContainerName = configuration.GetValue<string>("ContainerName");

                c.SwaggerDoc("v1", new OpenApiInfo { Title = ContainerName, Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // get the health check builder
            var healthCheckBuilder = services.AddHealthChecks();
            // get connection string
            var conString = ConnectionStringHelper.GetConnectionString(configuration);

            var ContainerName = configuration.GetValue<string>("ContainerName");

            //add health check for database
            healthCheckBuilder.AddMySql(conString, name: $"{ContainerName}-DB-healthcheck");

            // add health check for rabbit mq
            healthCheckBuilder.AddRabbitMQ($"amqp://{configuration["EventBusConnection"]}", name: $"{ContainerName}-RabbitMQ-healtcheck");

            return services;
        }
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent mapping from the 'sub' identifier to the name identifier
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var IamServiceUrl = configuration.GetValue<String>("IamServiceUrl");

            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(o =>
                {
                    o.Authority = IamServiceUrl;
                    var ContainerName = configuration.GetValue<string>("ContainerName");
                    o.Audience = $"{ContainerName}";
                    o.RequireHttpsMetadata = false;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            //Authorizations
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("ApiScope", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        var ContainerName = configuration.GetValue<string>("ContainerName");
                        policy.RequireClaim("scope", $"{ContainerName}");
                    });
                });

            return services;
        }
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            // add transient services
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        // Used to add multi tenant services
        public static IServiceCollection AddTenancy(this IServiceCollection services, IConfiguration configuration)
        {
            // add a scoped tenant object 
            services.AddScoped<TenantInfo>();

            // Use a connection per tenant
            // Add for all db context
            //services.AddScoped<CartContext>((serviceProvider) =>
            //{
            //    // get the tenant info
            //    var tenant = serviceProvider.GetRequiredService<TenantInfo>();
            //    // create the tenants connection string
            //    var connString = ConnectionStringHelper.GetConnectionString(configuration, tenant.Name);
            //    // create new options with the tenants connection string
            //    var options = new DbContextOptionsBuilder<CartContext>()
            //        .UseMySql(connString, ServerVersion.AutoDetect(connString))
            //        .Options;

            //    var context = new CartContext(options);

            //    return context;
            //});

            return services;
        }

        // Add razor pages configuration
        public static IServiceCollection AddRazorPages(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.RootDirectory = "/Views";
            });
            return services;
        }

        // Add Consul as a service
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            //register service with cosnul
            services.AddSingleton<IConsulClient, ConsulClient>(c => new ConsulClient(config =>
            {
                var consulUrl = configuration.GetValue<string>("ConsulUrl");
                config.Address = new Uri(consulUrl);
            }));

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder application, IConfiguration configuration)
        {
            // get logger
            var logger = application.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("CustomStartupExtensions.UseConsul");
            // get the consul client service
            var consulClient = application.ApplicationServices.GetRequiredService<IConsulClient>();
            // get the lifetime of the application
            var appLifetime = application.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            var serviceUrl = new Uri(configuration.GetValue<string>("ServiceUrl", "http://localhost:5401"));
            // TODO: Set tenant name on creation
            var TenantName = "one";
            var ContainerName = configuration.GetValue<string>("ContainerName");
            var serviceName = $"{TenantName}:{ContainerName}";
            var randUuid = Guid.NewGuid();
            var uniqueServiceId = $"{serviceName}:RandomID";
            // consul registartion
            // TODO: Fetch information dynamically
            var reg = new AgentServiceRegistration()
            {
                ID = uniqueServiceId,
                Name = serviceName,
                Address = $"{serviceUrl.Host}",
                Port = serviceUrl.Port
            };

            logger.LogInformation($"Registering \'{reg.Name}\' with Consul");
            // deregister service
            consulClient.Agent.ServiceDeregister(reg.ID).ConfigureAwait(true);
            // register service
            consulClient.Agent.ServiceRegister(reg).ConfigureAwait(true);

            appLifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation($"Unregistering \'{reg.Name}\' from Consul");
                consulClient.Agent.ServiceDeregister(reg.ID).ConfigureAwait(true);
            });

            return application;
        }

    }
}
