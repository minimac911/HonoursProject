﻿// <auto-generated />
using System;
using Cart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cart.Migrations
{
    [DbContext(typeof(CartContext))]
    partial class CartContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Cart.Models.CartDetails", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.HasKey("UserId")
                        .HasName("pk_cart_details");

                    b.ToTable("cart_details");
                });

            modelBuilder.Entity("Cart.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CartDetailsUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id")
                        .HasName("pk_cart_item");

                    b.HasIndex("CartDetailsUserId");

                    b.ToTable("cart_item");
                });

            modelBuilder.Entity("Cart.Models.CartItem", b =>
                {
                    b.HasOne("Cart.Models.CartDetails", "CartDetails")
                        .WithMany("Items")
                        .HasForeignKey("CartDetailsUserId");

                    b.Navigation("CartDetails");
                });

            modelBuilder.Entity("Cart.Models.CartDetails", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
