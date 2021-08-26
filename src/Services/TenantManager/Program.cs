using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TenantManager.Infastrucutre.Seed;

namespace TenantManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // run migrations for all tenants
            //TenantSeed.RunMigrations(Directory.GetCurrentDirectory());

            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
