using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Infastrucutre.Helper;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Models.Cart.DTO;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class CartController : MyController
    {
        private ICartService _cartService;
        private ICatalogService _catalogService;
        private IIdentityParser<ApplicationUser> _identityParser;

        public CartController(
            ICartService cartService, 
            ICatalogService catalogService,
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<MyController> logger) 
            : base(tenantManagerService, identityParser, logger)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            await ServiceReporting.Log($"Get cart");
            // get the user id 
            var user = _identityParser.Parse(HttpContext.User);
            // get the cart of user
            var cartDetails = await _cartService.GetCart(user);
            await ServiceReporting.Log("Display cart");
            // load view
            return View(cartDetails);
        }

        public async Task<IActionResult> AddItemToCart(int id, int qty)
        {
            await ServiceReporting.Log($"Add item to cart");
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

        public async Task<IActionResult> UpdateCart(UpdateCartItemDTO[] items)
        {
            await ServiceReporting.Log($"Update cart");
            if(items.Length == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            // get the user id 
            var user = _identityParser.Parse(HttpContext.User);
            var cartDetails = await _cartService.GetCart(user);

            foreach (UpdateCartItemDTO itm in items)
            {
                var cartItem = cartDetails.Items.FirstOrDefault(i => i.Id == itm.Id);
                cartItem.Quantity = itm.Quantity;
            }

            await _cartService.UpdateCart(user, cartDetails);

            return RedirectToAction("Index", "Cart");
        }
    }
}
