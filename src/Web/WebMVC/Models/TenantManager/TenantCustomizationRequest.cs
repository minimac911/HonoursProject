using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.TenantManager
{
    public class TenantCustomizationRequest
    {
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
    }
}
