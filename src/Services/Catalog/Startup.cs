using Autofac;
using Autofac.Extensions.DependencyInjection;
using Catalog.Data;
using Catalog.Extensions;
using Catalog.Helper;
using Catalog.Infastrucutre.Tenancy;
using EventBus;
using EventBus.EventBusRabbitMQ;
using EventBus.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;


namespace Catalog
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
            services
                .AddSecurity(Configuration)
                .AddMVCControllers(Configuration)
                .AddDbContext(Configuration)
                .AddEventBus(Configuration)
                .AddIntegrationEvents(Configuration)
                .AddTenancy(Configuration)
                .AddSwagger(Configuration)
                .AddCustomHealthChecks(Configuration);

            // create a container
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CatalogContext catalogContext, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Add tenancy middleware
            app.UseMiddleware<TenantInfoMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                    // require authorization for all controllers
                    //.RequireAuthorization("ApiScope");
                // add health check endpoint
                endpoints.MapHealthChecks("/hc/ready", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready")
                }) ;
                // add liveness health check endpoint
                endpoints.MapHealthChecks("/hc/liveness", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
            });


            // configrue the event bus
            ConfigureEventBus(app);
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            // add events
            //eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }
    }
}
