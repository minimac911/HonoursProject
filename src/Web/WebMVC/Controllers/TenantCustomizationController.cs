using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infastrucutre.Helper;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.TenantCustomizaiton;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class TenantCustomizationController : Controller
    {
        private ITenantManagerService _tenantManagerService;
        public TenantCustomizationController(ITenantManagerService tenantManagerService)
        {
            _tenantManagerService = tenantManagerService;
        }

        [HttpGet("TenantCustomization")]
        public async Task<IActionResult> Index()
        {
            await ServiceReporting.Log($"Get possible customizations");
            var customizations = await _tenantManagerService.GetAllCustomizationPoints();
            var data = new IndexViewModel
            {
                CustomizationPoints = customizations
            };
            await ServiceReporting.Log("Display possible customizaitons");
            return View(data);
        }

        [HttpGet("TenantCustomization/{id}")]
        public async Task<IActionResult> ViewCustomizationPoint(int id)
        {
            await ServiceReporting.Log($"View specific possible customization");
            var customization = await _tenantManagerService.GetSingleCustomizationPoint(id);
            await ServiceReporting.Log("Display specific possible customizaitons");
            return View(customization);
        }
    }
}
