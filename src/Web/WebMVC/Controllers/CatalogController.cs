using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Services;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Catalog;

namespace WebMVC.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService) => _catalogService = catalogService;

        public async Task<IActionResult> Index()
        {
            
            var items = await _catalogService.GetCatalogItems();

            var data = new IndexViewModel()
            {
                CatalogItems = items
            };

            return View(data);
        }
    }
}
