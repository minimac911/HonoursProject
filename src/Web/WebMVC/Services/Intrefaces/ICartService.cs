using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.Cart;

namespace WebMVC.Services.Intrefaces
{
    public interface ICartService
    {
        Task<CartDetails> GetCart(ApplicationUser userId);
        Task<bool> AddItemToCart(ApplicationUser userId, CartItemDTO newItemDto);
    }
}
