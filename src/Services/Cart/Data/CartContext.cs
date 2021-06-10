using Bogus;
using Bogus.Extensions;
using Cart.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options)
        {
        }

        public DbSet<CartDetails> CartDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<CartItem>().ToTable("cart_item");
            modelBuilder.Entity<CartDetails>().ToTable("cart_details");

            // Configure the PK
            modelBuilder.Entity<CartItem>().HasKey(itm => itm.Id).HasName("pk_cart_item");
            modelBuilder.Entity<CartDetails>().HasKey(x => x.Id).HasName("pk_cart_details");

            // Set the precision of the decimal
            modelBuilder.Entity<CartItem>()
                .Property(i => i.Price)
                .HasPrecision(19, 4);
        }
    }
}
