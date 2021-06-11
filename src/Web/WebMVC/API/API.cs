using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.API
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
            public static string GetCart(string baseUrl, int userId)
            {
                return $"{baseUrl}/{userId}";
            }

            public static string AddItemToCart(string baseUrl, int userId)
            {
                return $"{baseUrl}/{userId}/item";
            }
        }
    }
}
