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
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 6, 10, 8, 30, 42, 519, DateTimeKind.Utc).AddTicks(5096), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Intelligent Granite Chair", 50m, new DateTime(2021, 6, 10, 8, 30, 42, 519, DateTimeKind.Utc).AddTicks(9792) },
                    { 2, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5167), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Awesome Rubber Tuna", 52m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5185) },
                    { 3, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5257), "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Fantastic Wooden Shoes", 54m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5260) },
                    { 4, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5299), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Licensed Metal Computer", 49m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5302) },
                    { 5, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5358), "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Small Cotton Sausages", 51m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5362) },
                    { 6, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5396), "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Intelligent Granite Bacon", 57m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5398) },
                    { 7, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5428), "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Ergonomic Concrete Chicken", 53m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5431) },
                    { 8, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5459), "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Small Cotton Salad", 53m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5461) },
                    { 9, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5491), "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Handcrafted Wooden Shirt", 49m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5493) },
                    { 10, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5521), "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Unbranded Wooden Soap", 56m, new DateTime(2021, 6, 10, 8, 30, 42, 520, DateTimeKind.Utc).AddTicks(5523) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");
        }
    }
}
