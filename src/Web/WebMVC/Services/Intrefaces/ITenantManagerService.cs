using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.TenantManager;

namespace WebMVC.Services.Intrefaces
{
    public interface ITenantManagerService
    {
        Task<TenantCustomization> GetTenantCustomizaiton(TenantCustomizationRequest dto);
        Task<IList<TenantCustomization>> GetAllCustomizations();
        Task<string> RunCustomizationGET(TenantCustomization customization, string TenantName);
        Task<string> RunCustomizationPOST(TenantCustomization customization, string TenantName, HttpRequest req);
    }
}
