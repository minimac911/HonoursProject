using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class init : Migration
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
                    { 1, new DateTime(2021, 8, 4, 20, 4, 50, 689, DateTimeKind.Utc).AddTicks(4466), "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Handcrafted Plastic Chicken", 49m, 12, new DateTime(2021, 8, 4, 20, 4, 50, 689, DateTimeKind.Utc).AddTicks(9191) },
                    { 2, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4117), "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Ergonomic Soft Car", 54m, 1, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4125) },
                    { 3, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4176), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Generic Metal Pizza", 50m, 9, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4178) },
                    { 4, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4207), "The Football Is Good For Training And Recreational Purposes", "Generic Cotton Bacon", 52m, 1, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4209) },
                    { 5, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4239), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Fantastic Concrete Fish", 50m, 7, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4241) },
                    { 6, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4267), "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Fantastic Plastic Chips", 57m, 11, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4268) },
                    { 7, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4294), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Practical Soft Pizza", 54m, 14, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4296) },
                    { 8, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4323), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Handcrafted Fresh Fish", 55m, 20, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4325) },
                    { 9, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4360), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Sleek Metal Ball", 52m, 5, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4362) },
                    { 10, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4388), "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Licensed Wooden Ball", 50m, 18, new DateTime(2021, 8, 4, 20, 4, 50, 690, DateTimeKind.Utc).AddTicks(4390) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");
        }
    }
}
