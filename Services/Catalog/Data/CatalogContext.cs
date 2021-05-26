using Catalog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext (DbContextOptions<CatalogContext> options) : base(options)
        {
        }
        
        public DbSet<CatalogItem> CatalogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<CatalogItem>().ToTable("Item");

            // Configure the PK
            modelBuilder.Entity<CatalogItem>().HasKey(itm => itm.Id).HasName("PK_Item");
        }
    }
}
