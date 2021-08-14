using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenantManager.Models
{
    public class TenantCustomization
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        // ControllerName is used to store the name of the controller that is being customized
        public string ControllerName { get; set; }
        // MethodName used to store the name of the method that is being customized
        public string MethodName { get; set; }
        // store the api endpoint for the new microservice
        public string ApiEndpoint { get; set; }
        // is the customization active
        public bool IsActive { get; set; }

    }
}
