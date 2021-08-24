using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Services.Intrefaces;

namespace WebMVC.Controllers
{
    public class DebugMvcController : BaseController
    {
        public DebugMvcController(
            ITenantManagerService tenantManagerService, 
            IIdentityParser<ApplicationUser> identityParser, 
            ILogger<BaseController> logger)
            : base(tenantManagerService, identityParser, logger)
        {

        }

        [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
