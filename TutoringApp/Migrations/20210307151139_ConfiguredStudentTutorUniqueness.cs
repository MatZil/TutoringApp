using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class ConfiguredStudentTutorUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_StudentId",
                table: "StudentTutors");

            migrationBuilder.DropIndex(
                name: "IX_StudentTutorIgnores_StudentId",
                table: "StudentTutorIgnores");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_StudentId_TutorId",
                table: "StudentTutors",
                columns: new[] { "StudentId", "TutorId" },
                unique: true,
                filter: "[StudentId] IS NOT NULL AND [TutorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutorIgnores_StudentId_TutorId",
                table: "StudentTutorIgnores",
                columns: new[] { "StudentId", "TutorId" },
                unique: true,
                filter: "[StudentId] IS NOT NULL AND [TutorId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_StudentId_TutorId",
                table: "StudentTutors");

            migrationBuilder.DropIndex(
                name: "IX_StudentTutorIgnores_StudentId_TutorId",
                table: "StudentTutorIgnores");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_StudentId",
                table: "StudentTutors",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutorIgnores_StudentId",
                table: "StudentTutorIgnores",
                column: "StudentId");
        }
    }
}
