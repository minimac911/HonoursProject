using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.TenantManager;

namespace WebMVC.ViewModels.TenantCustomizaiton
{
    public class IndexViewModel
    {
        public IEnumerable<CustomizationPoint> CustomizationPoints { get; set; }
    }
}
