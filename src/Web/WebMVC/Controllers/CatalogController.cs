using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Services;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Catalog;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class CatalogController : MyController
    {
        private ICatalogService _catalogService;

        public CatalogController(
            ICatalogService catalogService, 
            ITenantManagerService tenantManagerService, 
            IIdentityParser<ApplicationUser> identityParser, 
            ILogger<MyController> logger)
            : base(tenantManagerService, identityParser, logger)
        {
            _catalogService = catalogService; 
        }

        public async Task<IActionResult> Index()
        {
            // Get Catalog items from CatalogService
            var items = await _catalogService.GetCatalogItems();
            // Create view model
            var data = new IndexViewModel()
            {
                CatalogItems = items
            };
            // load view
            return View(data);
        }
    }
}
