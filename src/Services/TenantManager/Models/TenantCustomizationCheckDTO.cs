using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenantManager.Models
{
    public record TenantCustomizationCheckDTO
    {
        [Required]
        public string ControllerName { get; set; }
        [Required]
        public string MethodName { get; set; }
    }
}
