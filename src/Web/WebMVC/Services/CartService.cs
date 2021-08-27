using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.Cart;
using WebMVC.Models.Cart.DTO;
using WebMVC.Services.Intrefaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebMVC.Services
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

        public async Task<CartDetails> GetCart(ApplicationUser user)
        {
            // get the url for the api endpoint
            var url = API.Cart.GetCart(_serviceUrl, user.Id);

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

        public async Task<bool> AddItemToCart(ApplicationUser user, CartItemDTO newItemDto)
        {
            // get the url for the api endpoint
            var url = API.Cart.AddItemToCart(_serviceUrl, user.Id);
            // encode new item to json
            var data = new StringContent(JsonSerializer.Serialize(newItemDto), Encoding.UTF8, "application/json");
            // post data 
            var response = await _httpClient.PostAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return true;
        }

        public async Task<bool> DeleteCart(ApplicationUser user)
        {
            // get the url for the api endpoint
            var url = API.Cart.DeleteCart(_serviceUrl, user.Id);
            // Delete
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return true;
        }

        public async Task<bool> UpdateCart(ApplicationUser user, CartDetails updateCart)
        {
            // get the url for the api endpoint
            var url = API.Cart.UpdateCart(_serviceUrl, user.Id);

            var data = new StringContent(JsonSerializer.Serialize(updateCart), Encoding.UTF8, "application/json");
            // get the response from the api 
            var response = await _httpClient.PutAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            return true;
        }
    }
}
