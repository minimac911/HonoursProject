using Order.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Models
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            this.Items = new List<OrderItem>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
