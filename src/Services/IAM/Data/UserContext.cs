using IAM.Helpers;
using IAM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Data
{
    public class UserContext : DbContext
    {
        public UserContext (DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<User>().ToTable("user");

            // Configure the PK
            modelBuilder.Entity<User>().HasKey(itm => itm.Id).HasName("pk_user");

            modelBuilder
                .Entity<User>()
                .HasData(
                    new User { Id = 1, Username = "a", Password = PasswordHasher.HashPassword("a") },
                    new User { Id = 2, Username = "b", Password = PasswordHasher.HashPassword("b") }
                );
        }
    }
}
