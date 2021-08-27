using CatalogCustomization.Infrastructure;
using CatalogCustomization.Models.Cart;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CatalogCustomization.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _serviceUrl;

        public CartService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var apiGatewayUrl = _configuration["ApiGatewayUrl"];
            _serviceUrl = $"{apiGatewayUrl}/api/cart";
        }

        public async Task<CartDetails> GetCart(string userId)
        {
            // get the url for the api endpoint
            var url = CoreAPI.Cart.GetCart(_serviceUrl, userId);

            try
            {
                // get the response from the api 
                var responseString = await _httpClient.GetStringAsync(url);

                var cartDetials = JsonSerializer.Deserialize<CartDetails>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return cartDetials;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        //public async Task<bool> AddItemToCart(ApplicationUser user, CartItemDTO newItemDto)
        //{
        //    // get the url for the api endpoint
        //    var url = API.Cart.AddItemToCart(_serviceUrl, user.Id);
        //    // encode new item to json
        //    var data = new StringContent(JsonSerializer.Serialize(newItemDto), Encoding.UTF8, "application/json");
        //    // post data 
        //    var response = await _httpClient.PostAsync(url, data);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception(response.ReasonPhrase);
        //    }

        //    return true;
        //}

        //public async Task<bool> DeleteCart(ApplicationUser user)
        //{
        //    // get the url for the api endpoint
        //    var url = API.Cart.DeleteCart(_serviceUrl, user.Id);
        //    // Delete
        //    var response = await _httpClient.DeleteAsync(url);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception(response.ReasonPhrase);
        //    }

        //    return true;
        //}

        public async Task<bool> UpdateCart(string userId, CartDetails updateCart)
        {
            // get the url for the api endpoint
            var url = CoreAPI.Cart.UpdateCart(_serviceUrl, userId);

            var data = new StringContent(JsonSerializer.Serialize(updateCart), Encoding.UTF8, "application/json");
            // get the response from the api 
            var response = await _httpClient.PutAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return true;
        }

        public async Task<bool> DeleteItemFromCart(string userId, int id)
        {
            // get the url for the api endpoint
            var url = CoreAPI.Cart.DeleteItemFromCart(_serviceUrl, userId, id);
            // Delete
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return true;
        }
    }
}
