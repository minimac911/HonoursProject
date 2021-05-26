using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "item");

            migrationBuilder.AddPrimaryKey(
                name: "PK_item",
                table: "item",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_item",
                table: "item");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "item",
                column: "Id");
        }
    }
}
