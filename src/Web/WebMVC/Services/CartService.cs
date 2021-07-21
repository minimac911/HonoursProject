using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Models.Cart;
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

        public async Task<CartDetails> GetCart(int userId)
        {
            // get the url for the api endpoint
            var url = API.API.Cart.GetCart(_serviceUrl, userId);

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

        public async Task<bool> AddItemToCart(int userId, CartItemDTO newItemDto)
        {
            // get the url for the api endpoint
            var url = API.API.Cart.AddItemToCart(_serviceUrl, userId);
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

    }
}
