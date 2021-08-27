using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models.Order;
using WebMVC.Models.Order.DTO;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _serviceUrl;

        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var apiGatewayUrl = _configuration["ApiGatewayUrl"];
            _serviceUrl = $"{apiGatewayUrl}/api/order";
        }

        public async Task<List<OrderDetails>> GetOrdersForUser()
        {
            // get the url for the api endpoint
            var url = API.Order.GetAllOrdersForUser(_serviceUrl);

            try
            {
                // get the response from the api 
                var responseString = await _httpClient.GetStringAsync(url);

                var orders = JsonSerializer.Deserialize<List<OrderDetails>>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return orders;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<bool> CreateOrder(OrderDTO orderDto)
        {
            // get the url for the api endpoint
            var url = API.Order.CreateOrder(_serviceUrl);
            // encode to json
            var data = new StringContent(JsonSerializer.Serialize(orderDto), Encoding.UTF8, "application/json");
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
