using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Models.Order.DTO;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Order;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class OrderController : MyController
    {
        private IOrderService _orderService;
        private ICatalogService _catalogService;
        private ICartService _cartService;
        private IIdentityParser<ApplicationUser> _identityParser;

        public OrderController(
            IOrderService orderService,
            ICatalogService catalogService,
            ICartService cartService,
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<MyController> logger)
            : base(tenantManagerService, identityParser, logger)
        {
            _orderService = orderService;
            _catalogService = catalogService;
            _cartService = cartService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersForUser();
            var data = new OrdersViewModel
            {
                Orders = orders
            };
            return View(data);
        }

        public async Task<IActionResult> CreateOrder()
        {
            // TODO: Add to aggregator
            // STEP 1: Get Cart info
            var user = _identityParser.Parse(HttpContext.User);
            CartDetails cart = await _cartService.GetCart(user);

            if(cart != null)
            {
                // STEP 2: Create order
                var order = new OrderDTO
                {
                    TotalPaid = cart.Total
                };

                foreach(CartItem item in cart.Items)
                {
                    var newOrderItem = new OrderItemDTO
                    {
                        ItemId = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    order.Items.Add(newOrderItem);
                }

                var orderCreated = await _orderService.CreateOrder(order);

                if(orderCreated == false)
                {
                    return RedirectToAction("Index", "Cart");
                }

                //STEP 3: Delete Cart
                // this should be done throuhg Event Bus
                await _cartService.DeleteCart(user);

                return RedirectToAction("Index", "Order");

            }
            return RedirectToAction("Index", "Cart");
        }
    }
}
