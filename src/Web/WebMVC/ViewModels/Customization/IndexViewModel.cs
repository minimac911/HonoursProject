using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.TenantManager;

namespace WebMVC.ViewModels.Customization
{
    public class IndexViewModel
    {
        public IEnumerable<TenantCustomization> Customizations { get; set; }
    }
}
