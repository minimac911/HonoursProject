using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_item",
                table: "item");

            migrationBuilder.AddPrimaryKey(
                name: "pk_item",
                table: "item",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_item",
                table: "item");

            migrationBuilder.AddPrimaryKey(
                name: "PK_item",
                table: "item",
                column: "Id");
        }
    }
}
