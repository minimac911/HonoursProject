using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenantManager.Models
{
    public class TenantCustomization
    {
        public int Id { get; set; }

        // ControllerName is used to store the name of the controller that is being customized
        public string ControllerName { get; set; }
        // MethodName used to store the name of the method that is being customized
        public string MethodName { get; set; }
        // store name of customization service for service discovery and dynamic service routing for ocelot
        public string ServiceName { get; set; }
        // The service end point 
        public string ServiceEndPoint{ get; set; }
        // is the customization active
        public bool IsActive { get; set; }

    }
}
