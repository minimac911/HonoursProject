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

            public static string DeleteCart(string baseUrl, string userId)
            {
                return $"{baseUrl}/{userId}";
            }
            public static string UpdateCart(string baseUrl, string userId)
            {
                return $"{baseUrl}/{userId}";
            }
        }

        public static class Order
        {
            public static string GetAllOrdersForUser(string baseUrl)
            {
                return $"{baseUrl}";
            }
            public static string GetSingleOrder(string baseUrl, int OrderId)
            {
                return $"{baseUrl}/{OrderId}";
            }

            public static string CreateOrder(string baseUrl)
            {
                return $"{baseUrl}";
            }
        }

        public static class TenantManager
        {
            public static string GetTenatCustomization(string baseUrl, string ControllerName, string MethodName)
            {
                return $"{baseUrl}/{ControllerName}/{MethodName}";
            }

            public static string GetTenatCustomization(string baseUrl, int id)
            {
                return $"{baseUrl}/{id}";
            }

            public static string GetAllCustomizations(string baseUrl)
            {
                return $"{baseUrl}";
            }

            // posibly add query data
            public static string RunTenantCustomizaton(string baseUrl, string TenantName, TenantCustomization customization)
            {
                // make sure that the service end point starts with a slash
                customization.ServiceEndPoint = 
                    customization.ServiceEndPoint.Substring(0,1) == "/" 
                    ? customization.ServiceEndPoint : "/"+customization.ServiceEndPoint;

                var url = $"{baseUrl}/{TenantName}:{customization.ServiceName + customization.ServiceEndPoint}";

                return url;
            }

            public static string GetAllCustomizationPoints(string baseUrl)
            {
                return $"{baseUrl}/customization_points";
            }

            public static string GetSingleCustomizationPoint(string baseUrl, int id)
            {
                return $"{baseUrl}/customization_points/{id}";
            }

            public static string UpdateTenantCustomization(string baseUrl)
            {
                return $"{baseUrl}/update_tenant_customization";
            }

            public static string CreateNewTenantCustomization(string baseUrl)
            {
                return $"{baseUrl}";
            }
        }
    }
}
