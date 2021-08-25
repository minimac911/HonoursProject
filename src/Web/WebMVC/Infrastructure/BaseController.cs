using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.TenantManager;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Customization;

namespace WebMVC.Infrastructure
{
    public class BaseController : Controller
    {
        private ITenantManagerService _tenantManagerService;
        private IIdentityParser<ApplicationUser> _identityParser;
        private ILogger<BaseController> _logger;

        public BaseController(
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<BaseController> logger)
        {
            _tenantManagerService = tenantManagerService;
            _identityParser = identityParser;
            _logger = logger;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = context.ActionDescriptor.RouteValues["controller"].ToString();
            var method = context.ActionDescriptor.RouteValues["action"].ToString();

            var tenantCustomiztionRequestInfo = new TenantCustomizationRequest 
            { 
                ControllerName = controllerName,
                MethodName = method
            };

            try
            {
                // get customization using method name and controller name
                var foundCustomization = await _tenantManagerService.GetTenantCustomizaiton(tenantCustomiztionRequestInfo);

                // if there is no customization then continue
                // else run customization and display new web page
                if (foundCustomization == null)
                {
                    await next();
                }
                else
                {
                    var html = await _tenantManagerService.RunCustomization(foundCustomization);
                    context.Result = View("CustomizationTemplate", new LoadCustomizationViewModel { HTML = html });
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error when checking for customization", ex.Message);
                context.Result = new BadRequestObjectResult("Invalid!");
                // go to next
                await next();
            }
        }
    }
}
