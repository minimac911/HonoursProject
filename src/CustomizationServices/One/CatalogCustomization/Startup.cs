using Autofac;
using Autofac.Extensions.DependencyInjection;
using CatalogCustomization.Infastrucutre.Startup;
using CatalogCustomization.Infrastructure.Http;
using CatalogCustomization.Services;
using EventBus.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            // ADD SERVICES HERE
            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<ICartService, CartService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services
                .AddSecurity(Configuration)
                .AddMVCControllers(Configuration)
                .AddDbContext(Configuration)
                .AddEventBus(Configuration)
                .AddIntegrationEvents(Configuration)
                .AddIdentityService(Configuration)
                .AddTenancy(Configuration)
                .AddSwagger(Configuration)
                .AddCustomHealthChecks(Configuration)
                .AddRazorPages(Configuration)
                .AddConsul(Configuration);

            // create a container
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Add tenancy middleware
            //app.UseMiddleware<TenantInfoMiddleware>();
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers();
            });

            // Configure the event bus
            ConfigureEventBus(app);

            app.UseConsul(Configuration);
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
