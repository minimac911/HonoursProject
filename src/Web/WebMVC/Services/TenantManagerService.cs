using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models.TenantManager;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Services
{
    public class TenantManagerService : ITenantManagerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _serviceUrl;
        private readonly string _customizationApiGatewayUrl;

        public TenantManagerService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            // get the main BL api gateway
            var apiGatewayUrl = _configuration["ApiGatewayUrl"];
            _serviceUrl = $"{apiGatewayUrl}/api/tenant_manager";
            // get the customization service gateway
            var customizationApiGatewayUrl = _configuration["CustomizationApiGatewayUrl"];
            _customizationApiGatewayUrl = $"{customizationApiGatewayUrl}";
        }

        public async Task<TenantCustomization> GetTenantCustomizaiton(TenantCustomizationRequest dto)
        {
            // get the url for the api endpoint
            var url = API.TenantManager.GetTenatCustomization(_serviceUrl, dto.ControllerName, dto.MethodName);

            try
            {
                // get the response from the api 
                var responseString = await _httpClient.GetStringAsync(url);

                var customizationDetails = JsonSerializer.Deserialize<TenantCustomization>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return customizationDetails;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Log.Logger.Error(ex.Message);
                return null;
            }
            catch (HttpRequestException ex)
            {
                Log.Logger.Error(ex.Message);
                throw;
            }
        }

        public async Task<string> RunCustomizationGET(TenantCustomization customization)
        {
            var url = API.TenantManager.RunTenantCustomizaton(
                _customizationApiGatewayUrl,
                customization);

            try
            {
                // get the response from the api 
                var responseString = await _httpClient.GetStringAsync(url);
                return responseString;
            }
            catch (HttpRequestException ex) 
            {
                Log.Logger.Error(ex.Message);
                throw;
            }
        }

        public async Task<string> RunCustomizationPOST(TenantCustomization customization, HttpRequest req)
        {
            var url = API.TenantManager.RunTenantCustomizaton(
                _customizationApiGatewayUrl,
                customization);

            try
            {
                string stringContent = string.Empty;

                try
                {
                    req.EnableBuffering();
                    req.Body.Position = 0;

                    using (var reader = new StreamReader(req.Body))
                    {
                        stringContent = await reader.ReadToEndAsync();

                        req.Body.Position = 0;
                    }

                }
                catch (Exception)
                {
                    throw;
                }

                var unescapedUrlData = Uri.UnescapeDataString(stringContent);
                var reqData = new StringContent(unescapedUrlData, Encoding.UTF8, "application/x-www-form-urlencoded");
                
                // Post data to url
                var response = await _httpClient.PostAsync(url, reqData);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                Log.Logger.Error(ex.Message);
                throw;
            }
        }
    }
}
