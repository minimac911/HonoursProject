using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _serviceUrl;

        public CatalogService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var apiGatewayUrl = _configuration["ApiGatewayUrl"];
            _serviceUrl = $"{apiGatewayUrl}/apigateway/catalog";
        }

        public async Task<List<CatalogItem>> GetCatalogItems()
        {
            // get the url for the api endpoint
            var url = API.API.Catalog.GetAllItems(_serviceUrl);

            // get the response from the api 
            var responseString = await _httpClient.GetStringAsync(url);

            var items = JsonSerializer.Deserialize<List<CatalogItem>>(responseString, new JsonSerializerOptions 
            {
                PropertyNameCaseInsensitive = true
            });

            return items;
        }
    }
}
