﻿// <auto-generated />
using IAM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IAM.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("IAM.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "user1@gmail.com",
                            Password = "$2a$12$j9rCuK.ccu.JozTbJsQ.KOFQKoBm4Cn5F1DinfXuWFhHsa84LMkbS",
                            Username = "user1"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user2@gmail.com",
                            Password = "$2a$12$YUfRhldtrWCa6yl7rRzEd.RcyIKH2oTTlmFXnf9M0ZAM6FIEHoPuO",
                            Username = "user2"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
