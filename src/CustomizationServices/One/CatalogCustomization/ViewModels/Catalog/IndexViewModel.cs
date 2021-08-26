using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogCustomization.Models;

namespace CatalogCustomization.ViewModels.Catalog
{
    public class IndexViewModel
    {
        public IEnumerable<CatalogItem> CatalogItems { get; set; }
        
        // TODO: add pagination
    }
}
