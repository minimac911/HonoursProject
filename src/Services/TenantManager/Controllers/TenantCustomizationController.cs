using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenantManager.Data;
using TenantManager.Infastrucutre.Helper;
using TenantManager.Models;

namespace TenantManager.Controllers
{
    [Route("api/tenant_manager")]
    [ApiController]
    [Authorize]
    public class TenantCustomizationController : Controller
    {
        private readonly TenantCustomizationContext _context;
        private readonly ILogger<TenantCustomizationController> _logger;

        public TenantCustomizationController(TenantCustomizationContext context, ILogger<TenantCustomizationController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        // GET: Tenat Customization
        [HttpGet("{ControllerName}/{MethodName}")]
        public async Task<ActionResult<TenantCustomization>> GetTenantCustomization(string ControllerName, string MethodName)
        {
            await ServiceReporting.Log($"Checking for tenant customization (C: {ControllerName}, M: {MethodName})");
            _logger.LogInformation($"Checking for tenant customization (C: {ControllerName}, M: {MethodName})");

            // get active customization using controller and method name
            var customization = await _context
                .TenantCustomizations
                .FirstOrDefaultAsync(x => 
                    x.ControllerName.Equals(ControllerName) 
                    && x.MethodName.Equals(MethodName)
                    && x.IsActive == true);

            // if no customization was found
            if(customization == null)
            {
                _logger.LogInformation($"No customization was found (C: {ControllerName}, M: {MethodName})");
                await ServiceReporting.Log($"No customization was found (C: {ControllerName}, M: {MethodName})");
                return NotFound();
            }

            _logger.LogInformation($"Customization was found (C: {ControllerName}, M: {MethodName})");
            await ServiceReporting.Log($"Customization was found (C: {ControllerName}, M: {MethodName})");
            return customization;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenantCustomization>> GetSingleTenantCustomization(int id)
        {
            await ServiceReporting.Log($"Get customizations for tenat id: {id}");
            // get active customization using controller and method name
            var customization = await _context
                .TenantCustomizations
                .FirstOrDefaultAsync(x => x.Id == id);

            // if no customization was found
            if (customization == null)
            {
                return NotFound();
            }

            return customization;
        }

        // GET: Tenat Customization
        [HttpGet]
        public async Task<ActionResult<IList<TenantCustomization>>> GetAllCustomizaitons()
        {
            await ServiceReporting.Log($"Get all teant customiations");
            var customizations = await _context
                .TenantCustomizations
                .ToListAsync();

            return customizations;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateNewTenantCustomization(TenantCustomization data)
        {
            await ServiceReporting.Log($"Create a new tenat customization");
            TenantCustomization newCust = new TenantCustomization
            {
                ControllerName = data.ControllerName,
                Description = data.Description,
                IsActive = true,
                MethodName = data.MethodName,
                ServiceEndPoint = data.ServiceEndPoint,
                ServiceName = data.ServiceName,
                Title = data.Title
            };

            // add the customization to the db
            _context.TenantCustomizations.Add(newCust);

            // save the customization to the database
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: customization points
        [HttpGet("customization_points")]
        public async Task<ActionResult<IList<CustomizationPoint>>> GetCustomizationsPoints()
        {
            await ServiceReporting.Log($"Get all customization points");
            var customizationPoints = await _context
                .CustomizationPoints
                .ToListAsync();

            return customizationPoints;
        }

        [HttpGet("customization_points/{id}")]
        public async Task<ActionResult<CustomizationPoint>> GetSingleCustomizationsPoints(int id)
        {
            await ServiceReporting.Log($"Get a single customization point ({id})");
            var customizationPoint = await _context
                .CustomizationPoints
                .FindAsync(id);

            return customizationPoint;
        }

        [HttpPost("update_tenant_customization")]
        public async Task<ActionResult<bool>> UpdateTenantCustomization(TenantCustomization data)
        {
            await ServiceReporting.Log($"Update tenant customization");
            var found = await _context.TenantCustomizations.FindAsync(data.Id);

            if (found == null) return NotFound();

            found.Description = data.Description ?? found.Description;
            found.MethodName = data.MethodName ?? found.MethodName;
            found.ControllerName = data.ControllerName ?? found.ControllerName;
            found.IsActive = data.IsActive;
            found.Title = data.Title ?? found.Title;

            _context.TenantCustomizations.Update(found);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return true;
        }
    }
}
