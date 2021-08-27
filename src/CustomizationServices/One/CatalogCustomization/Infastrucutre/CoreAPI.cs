using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Infrastructure
{
    public class CoreAPI
    {
        public static class Catalog
        {
            public static string GetAllItems(string baseUrl)
            {
                return $"{baseUrl}";
            }
            public static string GetSingleItem(string baseUrl, int id)
            {
                return $"{baseUrl}/{id}";
            }
        }

        public static class Cart
        {
            public static string GetCart(string baseUrl, string userId)
            {
                return $"{baseUrl}/{userId}";
            }

            public static string AddItemToCart(string baseUrl, string userId)
            {
                return $"{baseUrl}/{userId}/item";
            }
            public static string UpdateCart(string baseUrl, string userId)
            {
                return $"{baseUrl}/{userId}";
            }

            public static string DeleteItemFromCart(string baseUrl, string userId, int id)
            {
                return $"{baseUrl}/{userId}/{id}";
            }
        }

        public static class TenantManager
        {
            public static string GetTenatCustomization(string baseUrl, string ControllerName, string MethodName)
            {
                return $"{baseUrl}/{ControllerName}/{MethodName}";
            }

            // posibly add query data
            public static string RunTenantCustomizaton(string baseUrl, string serviceName, string ControllerName, string MethodName = null)
            {
                var url = $"{baseUrl}/{serviceName}/{ControllerName}";

                if (MethodName != null && MethodName != "Index")
                {
                    url += $"{MethodName}";
                }

                return url;
            }
        }
    }
}
