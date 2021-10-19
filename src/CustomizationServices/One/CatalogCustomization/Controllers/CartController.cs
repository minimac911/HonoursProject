using CatalogCustomization.Infastrucutre;
using CatalogCustomization.Infastrucutre.Helper;
using CatalogCustomization.Models.Cart.DTO;
using CatalogCustomization.Services;
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
    [Route("cart")]
    public class CartController : Controller
    {
        private IIdentityService _identityService;
        private ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(IIdentityService identityService,
            ICartService cartService,
            ILogger<CartController> logger)
        {
            _identityService = identityService;
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // get the user id 
            var userId = _identityService.GetUserId();
            await ServiceReporting.Log($"Get user's cart from core BL services");
            _logger.LogInformation("Get user's cart from core BL services");
            var cartDetails = await _cartService.GetCart(userId);
            await ServiceReporting.Log($"Return custom view for cart");
            _logger.LogInformation("Return custom view for cart");
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
            await ServiceReporting.Log($"Get user's cart from core BL services");
            _logger.LogInformation("Get user's cart from core BL services");
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
            _logger.LogInformation("Get user's cart from core BL services");
            await ServiceReporting.Log($"Get user's cart from core BL services");
            await _cartService.UpdateCart(userId, cartDetails);

            _logger.LogInformation("Redirect");
            return new RedirectResponse("Index", "Cart");
        }
    }
}
