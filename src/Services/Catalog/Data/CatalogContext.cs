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
            modelBuilder.Entity<CatalogItem>().ToTable("item");

            // Configure the PK
            modelBuilder.Entity<CatalogItem>().HasKey(itm => itm.Id).HasName("pk_item");

            // Seeding test data for tables
            modelBuilder.Entity<CatalogItem>().HasData(new CatalogItem { Id = 1, Name = "Name A", Price = 199.99m,  Description = "Description A"});

        }
    }
}
