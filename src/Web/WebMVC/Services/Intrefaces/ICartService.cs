using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.Cart;

namespace WebMVC.Services.Intrefaces
{
    public interface ICartService
    {
        Task<CartDetails> GetCart(int userId);
        Task<bool> AddItemToCart(int userId, CartItemDTO newItemDto);
    }
}
