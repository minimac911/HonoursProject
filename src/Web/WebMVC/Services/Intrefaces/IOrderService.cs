using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Order;
using WebMVC.Models.Order.DTO;

namespace WebMVC.Services.Intrefaces
{
    public interface IOrderService
    {
        Task<List<OrderDetails>> GetOrdersForUser();
        Task<bool> CreateOrder(OrderDTO orderDto);
    }
}
