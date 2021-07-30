using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    public class DebugController : Controller
    {
        [Authorize( AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
