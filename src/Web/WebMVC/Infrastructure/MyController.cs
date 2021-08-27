using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebMVC.Exceptions;
using WebMVC.Models;
using WebMVC.Models.TenantManager;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Customization;

namespace WebMVC.Infrastructure
{
    public class MyController : Controller
    {
        private ITenantManagerService _tenantManagerService;
        private IIdentityParser<ApplicationUser> _identityParser;
        private ILogger<MyController> _logger;

        public MyController(
            ITenantManagerService tenantManagerService,
            IIdentityParser<ApplicationUser> identityParser,
            ILogger<MyController> logger)
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
                var result = await GetAndRunTenantCustomization(context, tenantCustomiztionRequestInfo);
                context.Result = result;
            }
            catch (TenantCustomizationNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                // go to next
                await next();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Error when checking for customization", ex.Message);
                context.Result = new BadRequestObjectResult(ex.Message);
            }
        }

        private async Task<ActionResult> GetAndRunTenantCustomization(ActionExecutingContext context, TenantCustomizationRequest data)
        {
            // get customization using method name and controller name
            var foundCustomization = await _tenantManagerService.GetTenantCustomizaiton(data);

            // if there is no customization then continue
            // else run customization and display new web page
            if (foundCustomization == null)
            {
                throw new TenantCustomizationNotFoundException();
            }

            // get the http method e.g. GET, POST, DELETE , PUT
            var httpMethodFromRequest = context.HttpContext.Request.Method;
            var html = "";
            try 
            {
                switch (httpMethodFromRequest)
                {
                    case "GET":
                        html = await _tenantManagerService.RunCustomizationGET(foundCustomization);
                        break;
                    case "POST":
                        var requestBody = context.HttpContext.Request;
                        html = await _tenantManagerService.RunCustomizationPOST(foundCustomization, requestBody);
                        break;
                    default:
                        return NotFound();
                }
            }
            catch(HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            
            return View("CustomizationTemplate", new LoadCustomizationViewModel { HTML = html });

        }
    }
}
