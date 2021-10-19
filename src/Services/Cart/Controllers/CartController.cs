using Cart.Data;
using Cart.Infastrucutre.Helper;
using Cart.Models;
using Cart.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : Controller
    {

        private readonly CartContext _context;

        public CartController(CartContext context)
        {
            _context = context;
        }

        // GET: The cart inforamtion for the user
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDetails>> GetCart(string userId)
        {
            await ServiceReporting.Log($"Get cart");
            // get the cart details for the user
            var foundCart = await _context.CartDetails
                .Include(a => a.Items)
                .FirstOrDefaultAsync(cd => cd.UserId == userId);

            // if there was no cart found
            if (foundCart == null)
            {
                return NotFound();
            }

            return foundCart;
        }

        // POST: Add item to cart
        [HttpPost("{userId}/item")]
        public async Task<ActionResult> AddItemToCart(string userId, CartItemDTO newItemDto)
        {
            await ServiceReporting.Log($"Add item to cart");
            var foundCartDetails = await _context.CartDetails.FindAsync(userId);

            CartItem newItem = new CartItem
            {
                ItemId = newItemDto.ItemId,
                Name = newItemDto.Name,
                Description = newItemDto.Description,
                Price = newItemDto.Price,
                Quantity = newItemDto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // if there are no cart details
            if (foundCartDetails == null)
            {
                CartDetails newCartDetails = new CartDetails
                {
                    UserId = userId,
                    Total = (newItemDto.Price*newItemDto.Quantity)
                };

                newItem.CartDetails = newCartDetails;

                newCartDetails.Items.Add(newItem);

                _context.CartDetails.Add(newCartDetails);
            }
            // if there is already a cart
            else
            {
                // add the details to the item 
                newItem.CartDetails = foundCartDetails;
                // add the item to the cart
                foundCartDetails.Items.Add(newItem);
                // update the total
                foundCartDetails.Total += (newItemDto.Price * newItemDto.Quantity);
            }
            
            // save the item to the database
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: The cart inforamtion for the user
        [HttpGet("{userId}/item/{cartItemId}")]
        public async Task<ActionResult<CartItem>> GetCartItem(string userId, int cartItemId)
        {
            await ServiceReporting.Log($"Get item from cart");
            // get teh details of the item
            var foundItem = await _context.CartItems.FindAsync(cartItemId);
            
            // if there was no item found
            if (foundItem == null)
            {
                return NotFound();
            }

            // if the cart item does not belong to the specified user
            if (foundItem.CartDetails.UserId == userId) 
            {
                return NotFound();
            }

            return foundItem;
        }

        // GET: The cart inforamtion for the user
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteCart(string userId)
        {
            await ServiceReporting.Log($"Delete cart");
            // get teh details of the item
            var foundCart = await _context.CartDetails.FindAsync(userId);

            // if there was no item found
            if (foundCart == null)
            {
                return NotFound();
            }

            var itemsInCart = await _context.CartItems.Where(i => i.CartDetails.UserId == userId).ToListAsync();

            foreach(CartItem item in itemsInCart)
            {
                _context.CartItems.Remove(item);
            }
            await _context.SaveChangesAsync();

            _context.CartDetails.Remove(foundCart);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> DeleteItemFromCart(string userId, int id)
        {
            await ServiceReporting.Log($"Delete item from cart");
            // get teh details of the item
            var foundCart = await _context.CartDetails.Include(inc => inc.Items).FirstOrDefaultAsync(c => c.UserId == userId);

            // if there was no item found
            if (foundCart == null)
            {
                return NotFound();
            }

            var itemToDelete = foundCart.Items.FirstOrDefault(d => d.Id == id);
            
            if(itemToDelete == null)
            {
                return NotFound();
            }

            foundCart.Items.Remove(itemToDelete);
            // update total
            foundCart.Total = CalculateCartTotal(foundCart);

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPut("{userId}")]
        public async Task<ActionResult<CartDetails>> UpdateCart(string userId, CartDetails cartDetails)
        {
            await ServiceReporting.Log($"Update cart");
            // get teh details of the item
            var foundCart = await _context.CartDetails.FindAsync(userId);

            // if there was no item found
            if (foundCart == null)
            {
                return NotFound();
            }

            foundCart.Items = cartDetails.Items;

            foundCart.Total = CalculateCartTotal(foundCart);

            _context.CartDetails.Update(foundCart);

            await _context.SaveChangesAsync();

            return foundCart;
        }

        private decimal CalculateCartTotal(CartDetails cart)
        {
            decimal total = 0;
            
            foreach(CartItem item in cart.Items)
            {
                total += (item.Price * item.Quantity);
            }

            return total;
        }

        private async Task<bool> DoesCartExsist(int userId)
        {
            var cartDetailsFound = await _context.CartDetails.FindAsync(userId);

            if(cartDetailsFound != null)
            {
                return false;
            }

            return true;
        }
    }
}
