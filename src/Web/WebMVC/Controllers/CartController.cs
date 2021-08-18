using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class CartController : BaseController
    {
        private ICartService _cartService;
        private ICatalogService _catalogService;
        private IIdentityParser<ApplicationUser> _identityParser;

        public CartController(
            ICartService cartService, 
            ICatalogService catalogService,
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<BaseController> logger) 
            : base(tenantManagerService, identityParser, logger)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            // get the user id 
            var user = _identityParser.Parse(HttpContext.User);
            var cartDetails = await _cartService.GetCart(user);
            return View(cartDetails);
        }

        public async Task<IActionResult> AddItemToCart(int id, int qty)
        {
            // TODO: ApiGateway Agregation
            // STEP 1: Get item
            var item = await _catalogService.GetSingleCatalogItemById(id);
            if(item?.Id != null)
            {
                // get the user id 
                var user = _identityParser.Parse(HttpContext.User);
                var newCartItem = new CartItemDTO
                {
                    ItemId = item.Id,
                    Description = item.Description,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = qty
                };
                // STEP 2: Add item to cart
                await _cartService.AddItemToCart(user, newCartItem);
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
