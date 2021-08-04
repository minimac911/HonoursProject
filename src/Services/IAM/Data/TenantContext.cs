using IAM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Data
{
    public class TenantContext : DbContext
    {
        public TenantContext(DbContextOptions<TenantContext> options) : base(options)
        {
        }

        public DbSet<TenantInfo> Tenant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<TenantInfo>().ToTable("tenant");

            // Configure the PK
            modelBuilder.Entity<TenantInfo>()
                .HasKey(itm => itm.Id)
                .HasName("pk_tenant");

            // make guid auto increment
            modelBuilder.Entity<TenantInfo>()
                .Property(x => x.Id)
                .HasDefaultValueSql("(UUID())");

           //modelBuilder.Entity<TenantInfo>().HasData(
           //    new TenantInfo() { Id = Guid.NewGuid(), Name = "Shop One" },
           //    new TenantInfo() { Id= Guid.NewGuid(), Name = "Shop Two"}
           //);

        }

    }
}
