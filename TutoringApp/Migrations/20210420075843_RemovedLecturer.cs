using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class RemovedLecturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2365ab4-0cf5-48ae-8216-b3f28cf4cd9f");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d2365ab4-0cf5-48ae-8216-b3f28cf4cd9f", "781deac4-300c-40b4-84dc-d5e15697c752", "Lecturer", "LECTURER" });
        }
    }
}
