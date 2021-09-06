using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebMVC.Helpers
{
    public class RedirectResponse
    {
        public string controller { get; set; }
        public string action { get; set; }
        public object data { get; set; }
    }
}
