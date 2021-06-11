using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: Change user id 
            var userId = 1;
            var cartDetails = await _cartService.GetCart(userId);
            return View(cartDetails);
        }

        public async Task<IActionResult> AddItemToCart(int id, int qty)
        {
            // TODO: ApiGateway Agregation
            // STEP 1: Get item
            var item = await _catalogService.GetSingleCatalogItemById(id);
            if(item?.Id != null)
            {
                var userId = 1;
                var newCartItem = new CartItemDTO
                {
                    ItemId = item.Id,
                    Description = item.Description,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = qty
                };
                // STEP 2: Add item to cart
                await _cartService.AddItemToCart(userId, newCartItem);
            }
            return RedirectToAction("Index", "Cart");
        }
    }
}
