using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IAM.Data;
using IAM.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IAM
{
    public class SeedData
    {
        private static List<TenantInfo> SAMPLE_TENANTS = new List<TenantInfo>()
        {
            new TenantInfo(){  Id = Guid.NewGuid(), Name = "one"},
            new TenantInfo(){  Id = Guid.NewGuid(), Name = "two"},
        };

        private static List<ApplicationUser> SAMPLE_USERS = new List<ApplicationUser>()
        {
            new ApplicationUser()
            {
                UserName = "UserOne",
                TenantId = SAMPLE_TENANTS.Find(x => x.Name == "one").Id,
            },
            new ApplicationUser()
            {
                UserName = "UserTwo",
                TenantId = SAMPLE_TENANTS.Find(x => x.Name == "two").Id,
            }
        };

        private static string UNIVERSAL_PASSWORD_FOR_USERS = "Pass123$";

        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
               );
            services.AddDbContext<TenantContext>(options =>
               options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
               );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    /*****************
                    * Seed Tenants
                    *****************/
                    var tenantContext = scope.ServiceProvider.GetService<TenantContext>();
                    tenantContext.Database.Migrate();

                    // get tenant one
                    var tenantOne = SAMPLE_TENANTS.Find(x => x.Name == "one");

                    var foundTenantOne = tenantContext.Tenant.FirstOrDefault(x => x.Name == tenantOne.Name);
                    if (foundTenantOne == null)
                    {
                        try
                        {
                            tenantContext.Tenant.Add(tenantOne);
                            tenantContext.SaveChanges();
                            Log.Debug("Tenant \'one\' created");
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(ex, "Error when adding tenant two", ex.Message);
                        }
                    }
                    else
                    {
                        tenantOne = foundTenantOne;
                        Log.Debug("Tenant \'one\' already exists");
                    }

                    // get tenant two
                    var tenantTwo = SAMPLE_TENANTS.Find(x => x.Name == "two");
                  
                    var foundTenantTwo = tenantContext.Tenant.FirstOrDefault(x => x.Name == tenantTwo.Name);
                    if (foundTenantTwo == null)
                    {
                        try
                        {
                            tenantContext.Tenant.Add(tenantTwo);
                            tenantContext.SaveChanges();
                            Log.Debug("Tenant \'two\' created");
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(ex, "Error when adding tenant two", ex.Message);
                        }
                    }
                    else
                    {
                        tenantTwo = foundTenantTwo;
                        Log.Debug("Tenant \'two\' already exists");
                    }
                  


                    /*****************
                     * Seed users
                     *****************/
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var userOne = SAMPLE_USERS.Find(x => x.UserName == "UserOne");
                    // get user from db
                    var foundUser1 = userManager.FindByNameAsync(userOne.UserName).Result;
                    if (foundUser1 == null)
                    {
                        var result = userManager.CreateAsync(userOne, UNIVERSAL_PASSWORD_FOR_USERS).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("user1 created");
                    }
                    else
                    {
                        Log.Debug("user1 already exists");
                    }

                    var userTwo = SAMPLE_USERS.Find(x => x.UserName == "UserTwo");

                    var foundUserTwo = userManager.FindByNameAsync(userTwo.UserName).Result;
                    if (foundUserTwo == null)
                    {
                        var result = userManager.CreateAsync(userTwo, UNIVERSAL_PASSWORD_FOR_USERS).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("user2 created");
                    }
                    else
                    {
                        Log.Debug("user2 already exists");
                    }
                }
            }
        }
    }
}
