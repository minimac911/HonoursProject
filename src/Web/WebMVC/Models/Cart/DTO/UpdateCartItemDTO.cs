using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.Cart.DTO
{
    public class UpdateCartItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
