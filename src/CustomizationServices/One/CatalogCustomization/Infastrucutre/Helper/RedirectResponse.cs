using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatalogCustomization.Infastrucutre.Helper
{
    public class RedirectResponse
    {
        public string controller;
        public string action;
        public object data;

        public RedirectResponse(string controller, string action)
        {
            this.controller = controller;
            this.action = action;
            this.data = new Object();
        }
    }
}
