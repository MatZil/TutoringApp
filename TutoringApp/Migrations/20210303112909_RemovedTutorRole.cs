using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class RemovedTutorRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48b3c783-707f-46ea-9349-89a051066cc5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "48b3c783-707f-46ea-9349-89a051066cc5", "698a1fe5-0e2d-40db-bab2-7c6af6b24288", "Tutor", "TUTOR" });
        }
    }
}
