using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Helper
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString(IConfiguration configuration, string tenantName = null)
        {
            var connectionString = configuration.GetValue<string>("ConnectionString_debug");
            // if there is a tenant name
            if (tenantName != null)
            {
                connectionString = configuration.GetValue<string>($"ConnectionString_{tenantName.ToLower()}") ?? configuration.GetValue<String>($"ConnectionString_debug");
            }

            if (connectionString == null) connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            return connectionString;
        }
    }
}
