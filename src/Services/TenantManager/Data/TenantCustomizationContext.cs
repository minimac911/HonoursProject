using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenantManager.Models;

namespace TenantManager.Data
{
    public class TenantCustomizationContext : DbContext
    {
        public TenantCustomizationContext(DbContextOptions<TenantCustomizationContext> options) : base(options)
        {
        }

        public DbSet<TenantCustomization> TenantCustomizations{ get; set; }
        public DbSet<CustomizationPoint> CustomizationPoints{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<TenantCustomization>().ToTable("tenant_manager_customization");
            modelBuilder.Entity<CustomizationPoint>().ToTable("tenant_manager_customization_point");

            // Configure the PK
            modelBuilder.Entity<TenantCustomization>().HasKey(itm => itm.Id).HasName("tenant_manager_customization");
            modelBuilder.Entity<CustomizationPoint>().HasKey(itm => itm.Id).HasName("tenant_manager_customization_point");
        }
    }
}
