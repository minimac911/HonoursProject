using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        public static object JArray { get; private set; }

        static async Task Main(string[] args)
        {
            while (true)
            {
                var client = new HttpClient();
                await Extensions.HttpExtensions.HandleToken(client, "http://iam", "client", "secret", "catalog");

                // call api

                var response = await client.GetAsync("http://catalog-api/identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content.ToString());
                }
            }
        }
    }
}
