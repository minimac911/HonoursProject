using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Infastrucutre.Helper
{
    public class ServiceReporting
    {
        public static async Task Log(string message)
        {
            string ServiceName = "Catlog Customization";
            string currentDate = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "";
            string output = currentDate + " " + ServiceName + "|\t" + message;
            using StreamWriter file = new("./Logs/logs.txt", append: true);
            await file.WriteLineAsync(output);
        }
    }
}
