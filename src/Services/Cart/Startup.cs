using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.Interfaces;
using EventBus.EventBusRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using EventBus;
using Cart.Data;
using Microsoft.EntityFrameworkCore;

namespace Cart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMVCControllers(Configuration)
                .AddDbContext(Configuration)
                .AddEventBus(Configuration)
                .AddIntegrationEvents(Configuration)
                .AddSwagger(Configuration)
                .AddCustomHealthChecks(Configuration);

            // create a container
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CartContext cartContext, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configure the event bus
            ConfigureEventBus(app);

            // Migrate database
            try
            {
                logger.LogInformation("Migration: Testing connection to database...");
                if (cartContext.Database.CanConnect())
                {
                    logger.LogInformation("Migration: Database conected!");
                    logger.LogInformation("Migration: Database migration is running...");
                    cartContext.Database.Migrate();
                    logger.LogInformation("Migration: Database migration successful!");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Migration: Database migration failed! ({ex.Message})");
            }
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            // add events
            //eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }

    }

    public static class CustomStartupExtenstions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string mySqlConnectionStr = configuration["DockerConnectionString"];
            // if running through local host and not docker 
            if (mySqlConnectionStr == null)
            {
                mySqlConnectionStr = configuration["ConnectionStrings:DefaultConnection"];
            }
            services.AddDbContextPool<CartContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            return services;
        }

        public static IServiceCollection AddMVCControllers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            // get the health check builder
            var healthCheckBuilder = services.AddHealthChecks();
            // get connection string
            var conString = configuration["DockerConnectionString"] == null
                ? configuration["ConnectionStrings:DefaultConnection"]
                : configuration["DockerConnectionString"];

            //add health check for database
            healthCheckBuilder.AddMySql(conString, name: "Cart-DB-healthcheck");

            // add health check for rabbit mq
            healthCheckBuilder.AddRabbitMQ($"amqp://{configuration["EventBusConnection"]}", name: "Cart-RabbitMQ-healtcheck");

            return services;
        }
    }
}
