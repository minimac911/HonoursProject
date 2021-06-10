using Bogus;
using Bogus.Extensions;
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

            // Set the precision of the decimal
            modelBuilder.Entity<CatalogItem>()
                .Property(i => i.Price)
                .HasPrecision(19, 4);

            // Generate Bogus data
            var id = 1;
            var catalogItems = new Faker<CatalogItem>()
                .RuleFor(i => i.Id, f => id++)
                .RuleFor(i => i.Name, f => f.Commerce.ProductName())
                .RuleFor(i => i.Description, f => f.Commerce.ProductDescription())
                .RuleFor(i => i.Price, f => f.Commerce.Price(1).First())
                .RuleFor(i => i.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(i => i.UpdatedAt, f => DateTime.UtcNow);

            // generate 10 items
            modelBuilder
                .Entity<CatalogItem>()
                .HasData(catalogItems.GenerateBetween(10, 10));
        }
    }
}
