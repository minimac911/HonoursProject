using IAM.Helpers;
using IAM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the entity to the table 
            modelBuilder.Entity<User>().ToTable("user");

            // Configure the PK
            modelBuilder.Entity<User>().HasKey(usr => usr.Id).HasName("pk_user");

            // Create fake users
            modelBuilder
                .Entity<User>()
                .HasData( 
                new User { Id = 1, Email = "user1@gmail.com", Username = "user1", Password = PasswordHasher.HashPassword("user1")}, 
                new User { Id = 2, Email = "user2@gmail.com", Username = "user2", Password = PasswordHasher.HashPassword("user2") });
        }
    }
}
