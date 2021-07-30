using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Models
{
    public class CartDetails
    {
        public CartDetails()
        {
            this.Items = new List<CartItem>();
        }

        public string UserId { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}
