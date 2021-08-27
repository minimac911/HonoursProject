using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Cart;

namespace WebMVC.Models.Order.DTO
{
    public class OrderDTO
    {
        public OrderDTO()
        {
            this.Items = new List<OrderItemDTO>();
        }
        public decimal TotalPaid { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
