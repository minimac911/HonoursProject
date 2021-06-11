using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    UnitsLeft = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "item",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Price", "UnitsLeft", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 6, 11, 13, 46, 35, 636, DateTimeKind.Utc).AddTicks(4889), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Refined Granite Pants", 53m, 17, new DateTime(2021, 6, 11, 13, 46, 35, 636, DateTimeKind.Utc).AddTicks(9548) },
                    { 2, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3702), "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Unbranded Steel Tuna", 56m, 10, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3708) },
                    { 3, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3758), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Practical Fresh Cheese", 53m, 7, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3760) },
                    { 4, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3790), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Unbranded Cotton Ball", 53m, 15, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3792) },
                    { 5, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3821), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Intelligent Granite Hat", 52m, 20, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3822) },
                    { 6, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3934), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Sleek Granite Fish", 52m, 14, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3936) },
                    { 7, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3960), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Practical Rubber Pants", 53m, 13, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3962) },
                    { 8, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3997), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Practical Rubber Hat", 50m, 7, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(3999) },
                    { 9, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(4023), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Small Cotton Shirt", 50m, 2, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(4025) },
                    { 10, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(4050), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Generic Soft Sausages", 50m, 20, new DateTime(2021, 6, 11, 13, 46, 35, 637, DateTimeKind.Utc).AddTicks(4052) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");
        }
    }
}
