﻿// <auto-generated />
using System;
using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catalog.Migrations
{
    [DbContext(typeof(CatalogContext))]
    partial class CatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Catalog.Models.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id")
                        .HasName("pk_item");

                    b.ToTable("item");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 519, DateTimeKind.Utc).AddTicks(5096),
                            Description = "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J",
                            Name = "Intelligent Granite Chair",
                            Price = 50m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 519, DateTimeKind.Utc).AddTicks(9792)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5167),
                            Description = "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J",
                            Name = "Awesome Rubber Tuna",
                            Price = 52m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5185)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5257),
                            Description = "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support",
                            Name = "Fantastic Wooden Shoes",
                            Price = 54m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5260)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5299),
                            Description = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                            Name = "Licensed Metal Computer",
                            Price = 49m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5302)
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5358),
                            Description = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
                            Name = "Small Cotton Sausages",
                            Price = 51m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5362)
                        },
                        new
                        {
                            Id = 6,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5396),
                            Description = "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J",
                            Name = "Intelligent Granite Bacon",
                            Price = 57m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5398)
                        },
                        new
                        {
                            Id = 7,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5428),
                            Description = "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals",
                            Name = "Ergonomic Concrete Chicken",
                            Price = 53m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5431)
                        },
                        new
                        {
                            Id = 8,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5459),
                            Description = "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart",
                            Name = "Small Cotton Salad",
                            Price = 53m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5461)
                        },
                        new
                        {
                            Id = 9,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5491),
                            Description = "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients",
                            Name = "Handcrafted Wooden Shirt",
                            Price = 49m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5493)
                        },
                        new
                        {
                            Id = 10,
                            CreatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5521),
                            Description = "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive",
                            Name = "Unbranded Wooden Soap",
                            Price = 56m,
                            UpdatedAt = new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5523)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
