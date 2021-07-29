using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    public class AccountController : Controller
    {
        // Post logout form
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return new SignOutResult(new[]
            {
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            });

        }
    }
}
