using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.Order
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public decimal TotalPaid { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
