using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Exceptions
{
    public class TenantCustomizationNotFoundException : Exception
    {
        public TenantCustomizationNotFoundException() { }

        public TenantCustomizationNotFoundException(string ControllerName, string MethodName)
            : base($"No Tenant Customizaiton Was Found: {ControllerName}/{MethodName}")
        {}
    }
}
