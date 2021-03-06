using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Infastrucutre.Tenancy
{
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenant = context.RequestServices.GetRequiredService<TenantInfo>();
            // get the tenant id and tenant name
            var tenantId = context.User.Claims.FirstOrDefault(x => x.Type == "tenant")?.Value ?? throw new ArgumentException("tenant not in claims");
            var tenantName = context.User.Claims.FirstOrDefault(x => x.Type == "tenant_name")?.Value ?? throw new ArgumentException("tenant_name not in claims");

            // set the tenant id anbd th
            tenant.Id = new Guid(tenantId);
            tenant.Name = tenantName;

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
