using CatalogCustomization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogCustomization.Services
{
    public interface ICatalogService
    {
        Task<List<CatalogItem>> GetCatalogItems(); 
        Task<CatalogItem> GetSingleCatalogItemById(int id);
    }
}
