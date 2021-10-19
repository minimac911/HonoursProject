using CatalogCustomization.Services;
using CatalogCustomization.ViewModels.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Controllers
{
    [Authorize]
    [ApiController()]
    [Route("catalog")]
    public class CatalogController : Controller
    {
        private ICatalogService _catalogService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(
            ICatalogService catalogService,
            ILogger<CatalogController> logger)
        {
            _catalogService = catalogService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Get catalog from core BL services");
            var items = await _catalogService.GetCatalogItems();

            var data = new IndexViewModel()
            {
                CatalogItems = items
            };

            _logger.LogInformation("Return Custom Catalog View");
            return View(data);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> ViewItem(int ItemId)
        {
            _logger.LogInformation("Get a single catalog item from core BL services");
            var singleItem = await _catalogService.GetSingleCatalogItemById(ItemId);
            _logger.LogInformation("Return Custom item view");
            return View(singleItem);
        }
    }
}
