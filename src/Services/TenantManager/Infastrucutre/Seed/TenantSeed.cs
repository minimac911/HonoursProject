using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TenantManager.Infastrucutre.Seed
{
    public class TenantSeed
    {
        public static async void RunMigrations(string baseDirectory)
        {
            // get the configured tenants
            List<MigrateTenantInfo> tenants = GetConfiguredTenants(baseDirectory);

            // setup for parrallel tasks
            IEnumerable<Task> tasks = tenants.Select(t => MigrateDatabase(t));

            try
            {
                Log.Information("Starting migrations");
                await Task.WhenAll(tasks);
            }
            catch
            {
                Log.Warning("Async complete with errors ");
                return;
            }

            Log.Information("Migrations Complete.");
        }

        private static async Task MigrateDatabase(MigrateTenantInfo t)
        {
            //var options = new DbContextOptionsBuilder<CatalogContext>()
            //    .UseMySql(t.ConnectionString, ServerVersion.AutoDetect(t.ConnectionString))
            //    .Options;
            //try
            //{
            //    var context = new CatalogContext(options);

            //    await context.Database.MigrateAsync();
            //}catch(Exception ex)
            //{
            //    Log.Error(ex, "Error has occured during migration", ex.Message);
            //}
        }

        private static List<MigrateTenantInfo> GetConfiguredTenants(string baseDirectory)
        {
            // get a builder with the current
            var builder = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            // get migration info from appsettigns
            return config.GetSection(nameof(MigrateTenantInfo)).Get<List<MigrateTenantInfo>>();
        }
    }
}
