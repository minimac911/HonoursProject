﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services.Intrefaces
{
    public interface ICatalogService
    {
        Task<List<CatalogItem>> GetCatalogItems();
    }
}
