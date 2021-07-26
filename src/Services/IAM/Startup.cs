using Autofac;
using Autofac.Extensions.DependencyInjection;
using IAM.Data;
using IAM.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM
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
                .AddJwtAuthenticaiton(Configuration)
                .AddMVCControllers(Configuration)
                .AddDbContext(Configuration)
                .AddSwagger(Configuration)
                .AddCustomHealthChecks(Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext userContext, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IAM v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // add health check endpoint
                endpoints.MapHealthChecks("/hc/ready", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready")
                });
                // add liveness health check endpoint
                endpoints.MapHealthChecks("/hc/liveness", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
            });

            // Migrate database
            try
            {
                logger.LogInformation("Migration: Testing connection to database...");
                if (userContext.Database.CanConnect())
                {
                    logger.LogInformation("Migration: Database conected!");
                    logger.LogInformation("Migration: Database migration is running...");
                    userContext.Database.Migrate();
                    logger.LogInformation("Migration: Database migration successful!");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Migration: Database migration failed! ({ex.Message})");
            }
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
            services.AddDbContextPool<UserContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
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


        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IAM", Version = "v1" });
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
            healthCheckBuilder.AddMySql(conString, name: "IAM-DB-healthcheck");

            return services;
        }

        public static IServiceCollection AddJwtAuthenticaiton(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = true;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"])),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddScoped<ITokenBuilder, TokenBuilder>();
    
            return services;
        }
    }
}
