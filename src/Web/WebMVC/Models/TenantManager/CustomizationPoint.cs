using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.TenantManager
{
    public class CustomizationPoint
    {
        //TODO: Add parameters
        public int Id { get; set; }
        public string Description { get; set; }

        // ControllerName is used to store the name of the controller that is being customized
        public string ControllerName { get; set; }
        // MethodName used to store the name of the method that is being customized
        public string MethodName { get; set; }
        public string CodeSnippet { get; set; }
    }
}
