using Microsoft.EntityFrameworkCore;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<OrderItem>().ToTable("order_item");
            modelBuilder.Entity<OrderDetails>().ToTable("order_details");

            // Configure the PK
            modelBuilder.Entity<OrderItem>().HasKey(itm => itm.Id).HasName("pk_order_item");
            modelBuilder.Entity<OrderDetails>().HasKey(x => x.UserId).HasName("pk_order_details");

            // Set the precision of the decimal
            modelBuilder.Entity<OrderItem>()
                .Property(i => i.Price)
                .HasPrecision(19, 4);
            modelBuilder.Entity<OrderDetails>()
                .Property(i => i.TotalPaid)
                .HasPrecision(19, 4);
        }
    }
}
