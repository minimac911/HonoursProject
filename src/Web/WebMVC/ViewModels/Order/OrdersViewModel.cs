using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Order;

namespace WebMVC.ViewModels.Order
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderDetails> Orders { get; set; }
    }
}
