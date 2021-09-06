using CatalogCustomization.Infastrucutre;
using CatalogCustomization.Infastrucutre.Helper;
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
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<RedirectResponse>> UpdateCart([FromForm] UpdateCartItemDTO[] items)
        {
            if (items.Length == 0)
            {
                return new RedirectResponse("Index", "Cart");
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

            return new RedirectResponse("Index", "Cart");
        }
    }
}
