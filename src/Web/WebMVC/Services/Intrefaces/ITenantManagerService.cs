using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.TenantManager;

namespace WebMVC.Services.Intrefaces
{
    interface ITenantManagerService
    {
        Task<TenantCustomization> GetTenantCustomizaiton(TenantCustomizationRequest dto);
    }
}
