using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenantManager.Data;
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
            _logger.LogDebug($"Checking for tenant customization (C: {ControllerName}, M: {MethodName})");

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
                _logger.LogDebug($"No customization was found (C: {ControllerName}, M: {MethodName})");
                return NotFound();
            }

            _logger.LogDebug($"Customization was found (C: {ControllerName}, M: {MethodName}");
            return customization;
        }

        // GET: Tenat Customization
        [HttpGet]
        public async Task<ActionResult<IList<TenantCustomization>>> GetAllCustomizaitons()
        {
            var customizations = await _context
                .TenantCustomizations
                .ToListAsync();

            return customizations;
        }
    }
}
