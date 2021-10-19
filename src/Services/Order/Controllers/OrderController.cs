using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Infastrucutre.Helper;
using Order.Models;
using Order.Models.DTO;
using Order.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private IIdentityService _identityService;
        private OrderContext _context;

        public OrderController(IIdentityService identityService, OrderContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDetails>> GetAllOrders()
        {
            await ServiceReporting.Log($"Get all orders");
            // get the user id throuhg 
            var userId = _identityService.GetUserId();
            // Get all orders for the user
            var orders = await _context.OrderDetails
                .Where(o => o.UserId == userId)
                .Include(a => a.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync<OrderDetails>();

            return orders;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDTO order)
        {
            await ServiceReporting.Log($"Create order");
            // get the user id throuhg 
            var userId = _identityService.GetUserId();

            var newOrder = new OrderDetails 
            { 
                UserId = userId,
                TotalPaid = order.TotalPaid,
                CreatedAt = DateTime.UtcNow
            };

            // Add each item from the dto to the new order 
            foreach(OrderItemDTO itemDto in order.Items)
            {
                var newOrderItem = new OrderItem 
                { 
                    ItemId = itemDto.ItemId,
                    Name = itemDto.Name,
                    Description = itemDto.Description,
                    Price = itemDto.Price,
                    Quantity = itemDto.Quantity,
                    OrderDetails = newOrder
                };

                newOrder.Items.Add(newOrderItem);
            }
            
            _context.OrderDetails.Add(newOrder);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
