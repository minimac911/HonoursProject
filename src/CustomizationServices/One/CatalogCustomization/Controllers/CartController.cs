using CatalogCustomization.Models.Cart.DTO;
using CatalogCustomization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Controllers
{
    [Authorize]
    [ApiController()]
    [Route("cart")]
    public class CartController : Controller
    {
        private IIdentityService _identityService;
        private ICartService _cartService;

        public CartController(IIdentityService identityService,
            ICartService cartService)
        {
            _identityService = identityService;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // get the user id 
            var userId = _identityService.GetUserId();
            var cartDetails = await _cartService.GetCart(userId);
            return View(cartDetails);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart(UpdateCartItemDTO[] items)
        {
            if (items.Length == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            // get the user id 
            var userId = _identityService.GetUserId();
            var cartDetails = await _cartService.GetCart(userId);

            foreach (UpdateCartItemDTO itm in items)
            {
                var cartItem = cartDetails.Items.FirstOrDefault(i => i.Id == itm.Id);
                cartItem.Quantity = itm.Quantity;
                if(itm.Quantity == 0)
                {
                    await _cartService.DeleteItemFromCart(userId, itm.Id);
                    cartDetails = await _cartService.GetCart(userId);
                }
            }

            await _cartService.UpdateCart(userId, cartDetails);

            return RedirectToAction("Index", "Cart");
        }
    }
}
