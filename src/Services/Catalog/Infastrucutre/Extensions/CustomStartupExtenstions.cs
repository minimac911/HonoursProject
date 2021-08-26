using Autofac;
using Catalog.Data;
using Catalog.Helper;
using Catalog.Infastrucutre.Tenancy;
using EventBus;
using EventBus.EventBusRabbitMQ;
using EventBus.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

namespace Catalog.Extensions
{
    public static class CustomStartupExtenstions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string mySqlConnectionStr = ConnectionStringHelper.GetConnectionString(configuration);
            services.AddDbContextPool<CatalogContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // get the health check builder
            var healthCheckBuilder = services.AddHealthChecks();
            // get connection string
            var conString = ConnectionStringHelper.GetConnectionString(configuration, null);

            //add health check for database
            healthCheckBuilder.AddMySql(conString, name: "Catalog-DB-healthcheck");

            // add health check for rabbit mq
            healthCheckBuilder.AddRabbitMQ($"amqp://{configuration["EventBusConnection"]}", name: "Catalog-RabbitMQ-healtcheck");

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
                    o.Audience = "catalog";
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
                        policy.RequireClaim("scope", "catalog");
                    });
                });

            return services;
        }

        // Used to add multi tenant services
        public static IServiceCollection AddTenancy(this IServiceCollection services, IConfiguration configuration)
        {
            // add a scoped tenant object 
            services.AddScoped<TenantInfo>();

            // Use a connection per tenant
            // Add for all db context
            services.AddScoped<CatalogContext>((serviceProvider) =>
            {
                // get the tenant info
                var tenant = serviceProvider.GetRequiredService<TenantInfo>();
                // create the tenants connection string
                var connString = ConnectionStringHelper.GetConnectionString(configuration, tenant.Name);
                // create new options with the tenants connection string
                var options = new DbContextOptionsBuilder<CatalogContext>()
                    .UseMySql(connString, ServerVersion.AutoDetect(connString))
                    .Options;

                var context = new CatalogContext(options);

                return context;
            });

            return services;
        }

    }
}
