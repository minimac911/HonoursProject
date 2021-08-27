using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Models.DTO
{
    public class OrderDTO
    {
        public decimal TotalPaid { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
