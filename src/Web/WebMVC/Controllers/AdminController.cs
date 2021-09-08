using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Customization;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AdminController : Controller
    {
        private ITenantManagerService _tenantManagerService;
        public AdminController(ITenantManagerService tenantManagerService)
        {
            this._tenantManagerService = tenantManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> Customizations()
        {
            var customizations = await _tenantManagerService.GetAllCustomizations();
            var viewData = new IndexViewModel()
            {
                Customizations = customizations
            };
            return View(viewData);
        }
    }
}
