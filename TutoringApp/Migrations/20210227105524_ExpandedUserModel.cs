using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class ExpandedUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "GlobalSettings");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StudentCycle",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentYear",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StudyBranch",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ffbbf0f7-a44b-4507-9638-16f23fe8d45e", "aef4be17-59c2-4348-aa62-e3240fa214bf", "Student", "STUDENT" },
                    { "48b3c783-707f-46ea-9349-89a051066cc5", "698a1fe5-0e2d-40db-bab2-7c6af6b24288", "Tutor", "TUTOR" },
                    { "6e30ffbf-e691-4e41-9349-0404349a8367", "81347000-aeb1-42fb-be76-80d87e0709e0", "Admin", "ADMIN" },
                    { "d2365ab4-0cf5-48ae-8216-b3f28cf4cd9f", "781deac4-300c-40b4-84dc-d5e15697c752", "Lecturer", "LECTURER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48b3c783-707f-46ea-9349-89a051066cc5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e30ffbf-e691-4e41-9349-0404349a8367");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2365ab4-0cf5-48ae-8216-b3f28cf4cd9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffbbf0f7-a44b-4507-9638-16f23fe8d45e");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentCycle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudyBranch",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSettings_Name",
                table: "GlobalSettings",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
