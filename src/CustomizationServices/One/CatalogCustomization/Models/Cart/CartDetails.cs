using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Models.Cart
{
    public class CartDetails
    {
        public decimal Total { get; set; }
        public virtual List<CartItem> Items { get; set; }
    }
}
