using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Helpers
{
    public static class JwtClaimHelper
    {
        public static string GetTenantName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(x => x.Type == "tenant_name").Value ?? null;
        }
    }
}
