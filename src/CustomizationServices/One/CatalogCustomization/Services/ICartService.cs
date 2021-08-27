using CatalogCustomization.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Services
{
    public interface ICartService
    {
        Task<CartDetails> GetCart(string userId);
        //Task<bool> AddItemToCart(ApplicationUser userId, CartItemDTO newItemDto);
        //Task<bool> DeleteCart(ApplicationUser user);
        Task<bool> UpdateCart(string userId, CartDetails updateCart);
        Task<bool> DeleteItemFromCart(string userId, int id);
    }
}
