using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Models.Cart.DTO;

namespace WebMVC.Services.Intrefaces
{
    public interface ICartService
    {
        Task<CartDetails> GetCart(ApplicationUser userId);
        Task<bool> AddItemToCart(ApplicationUser userId, CartItemDTO newItemDto);
        Task<bool> DeleteCart(ApplicationUser user);
        Task<bool> UpdateCart(ApplicationUser user, CartDetails updateCart);

    }
}
