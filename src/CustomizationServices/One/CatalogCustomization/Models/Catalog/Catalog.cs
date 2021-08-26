using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Models
{
    public record Catalog
    {
        public List<CatalogItem> Items { get; init; }
    }
}
