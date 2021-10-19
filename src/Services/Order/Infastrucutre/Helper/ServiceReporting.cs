﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Infastrucutre.Helper
{
    public class ServiceReporting
    {
        public static async Task Log(string message)
        {
            string ServiceName = "Order API";
            string currentDate = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "";
            string output = currentDate + " " + ServiceName + "\t\t|\t" + message;
            using StreamWriter file = new("./Logs/logs.txt", append: true);
            await file.WriteLineAsync(output);
        }
    }
}
