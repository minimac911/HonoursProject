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
                return $"{baseUrl}/items";
            }
        }
    }
}
