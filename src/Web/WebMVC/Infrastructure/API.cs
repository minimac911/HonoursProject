using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.TenantManager;

namespace WebMVC.Infrastructure
{
    public class API
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
        }

        public static class TenantManager
        {
            public static string GetTenatCustomization(string baseUrl, string ControllerName, string MethodName)
            {
                return $"{baseUrl}/{ControllerName}/{MethodName}";
            }

            // posibly add query data
            public static string RunTenantCustomizaton(string baseUrl, TenantCustomization customization)
            {
                // make sure that the service end point starts with a slash
                customization.ServiceEndPoint = 
                    customization.ServiceEndPoint.Substring(0,1) == "/" 
                    ? customization.ServiceEndPoint : "/"+customization.ServiceEndPoint;

                var url = $"{baseUrl}/{customization.ServiceName + customization.ServiceEndPoint}";

                return url;
            }
        }
    }
}
