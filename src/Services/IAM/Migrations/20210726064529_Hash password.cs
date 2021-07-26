using Microsoft.EntityFrameworkCore.Migrations;

namespace IAM.Migrations
{
    public partial class Hashpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$j9rCuK.ccu.JozTbJsQ.KOFQKoBm4Cn5F1DinfXuWFhHsa84LMkbS");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$12$YUfRhldtrWCa6yl7rRzEd.RcyIKH2oTTlmFXnf9M0ZAM6FIEHoPuO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "user1");

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "user2");
        }
    }
}
