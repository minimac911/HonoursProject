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
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class CatchAllController : MyController
    {
        public CatchAllController(
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<MyController> logger)
            : base(tenantManagerService, identityParser, logger)
        { }
        /**
         * This route is used to catch all routes that are not mathced
         * It is used so that the Tenant Manager can still be checked through the parent class
         * if no customization was found then a NotFound page will be shown
         */
        [Route("/{**catchAll}", Order = int.MaxValue)]
        public IActionResult Index()
        {
            return View("NotFound");
        }
    }
}
