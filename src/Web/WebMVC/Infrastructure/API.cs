using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
