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
    public class TestTenantCustomizationController : MyController
    {
        public TestTenantCustomizationController(
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<MyController> logger)
            : base(tenantManagerService, identityParser, logger)
        {}

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
