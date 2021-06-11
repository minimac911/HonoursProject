using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Models
{
    public class CartDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CartItem CartItem { get; set; }

    }
}
