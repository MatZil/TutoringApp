using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class StudentTutorModuleScope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_StudentId_TutorId",
                table: "StudentTutors");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "StudentTutors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_ModuleId",
                table: "StudentTutors",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_StudentId_TutorId_ModuleId",
                table: "StudentTutors",
                columns: new[] { "StudentId", "TutorId", "ModuleId" },
                unique: true,
                filter: "[StudentId] IS NOT NULL AND [TutorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTutors_Modules_ModuleId",
                table: "StudentTutors",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTutors_Modules_ModuleId",
                table: "StudentTutors");

            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_ModuleId",
                table: "StudentTutors");

            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_StudentId_TutorId_ModuleId",
                table: "StudentTutors");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "StudentTutors");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_StudentId_TutorId",
                table: "StudentTutors",
                columns: new[] { "StudentId", "TutorId" },
                unique: true,
                filter: "[StudentId] IS NOT NULL AND [TutorId] IS NOT NULL");
        }
    }
}
