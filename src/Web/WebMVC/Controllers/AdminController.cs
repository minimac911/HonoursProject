using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infastrucutre.Helper;
using WebMVC.Models;
using WebMVC.Models.TenantManager;
using WebMVC.Services.Intrefaces;
using WebMVC.ViewModels.Customization;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class AdminController : Controller
    {
        private ITenantManagerService _tenantManagerService;
        public AdminController(ITenantManagerService tenantManagerService)
        {
            this._tenantManagerService = tenantManagerService;
        }

        [HttpGet("Customizations")]
        public async Task<IActionResult> Customizations()
        {
            await ServiceReporting.Log($"View customizations");
            var customizations = await _tenantManagerService.GetAllCustomizations();
            var viewData = new IndexViewModel()
            {
                Customizations = customizations
            };
            await ServiceReporting.Log("Display customization");
            return View(viewData);
        }

        [HttpGet("Customizations/{id}")]
        public async Task<IActionResult> ViewCustomization(int id)
        {
            await ServiceReporting.Log($"View single customization");
            var customizations = await _tenantManagerService.GetTenantCustomizaiton(id);
      
            await ServiceReporting.Log("Display single customization");
            return View(customizations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ServiceReporting.Log("View to create new customization").Wait();
            return View("ViewNewCustomization");
        }

        [HttpGet("Customizations/{id}/edit")]
        public async Task<IActionResult> ViewEditCustomization(int id)
        {
            ServiceReporting.Log("View edit customization").Wait();
            var customizations = await _tenantManagerService.GetTenantCustomizaiton(id);

            return View(customizations);
        }


        public async Task<IActionResult> UpdateActiveStatusForCustomization(int id)
        {
            await ServiceReporting.Log("Make customization active");
            var cust = await _tenantManagerService.GetTenantCustomizaiton(id);

            cust.IsActive = !cust.IsActive;

            await _tenantManagerService.UpdateTenantCustomization(cust);

            return RedirectToAction("ViewCustomization", "Admin", new { id = id });
        }

        public async Task<IActionResult> UpdateCustomization(
            int id, 
            string title, 
            string description, 
            string service, 
            string controller, 
            string method, 
            string endPoint)
        {
            await ServiceReporting.Log("Update customization");
            var cust = await _tenantManagerService.GetTenantCustomizaiton(id);

            if (title != null)
            {
                cust.Title = title;
            }
            if (description != null)
            {
                cust.Description = description;
            }
            if (service != null)
            {
                cust.ServiceName = service;
            }
            if (controller != null)
            {
                cust.ControllerName = controller;
            }
            if (method != null)
            {
                cust.MethodName = method;
            }
            if (endPoint!= null)
            {
                cust.ServiceEndPoint = endPoint;
            }

            await _tenantManagerService.UpdateTenantCustomization(cust);

            return RedirectToAction("ViewCustomization", "Admin", new { id = id });
        }

        public async Task<IActionResult> CreateCustomization(
            string title,
            string description,
            string service,
            string controller,
            string method,
            string endPoint)
        {
            await ServiceReporting.Log("Create customization");
            var cust = new TenantCustomization
            {
                Title = title,
                Description = description,
                ServiceName = service,
                ServiceEndPoint = endPoint,
                ControllerName = controller,
                MethodName = method
            };

            await _tenantManagerService.CreateNewTenantCustomization(cust);

            return RedirectToAction("Customizations", "Admin");
        }
    }
}
