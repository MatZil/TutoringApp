using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class AddedModuleToTutoringSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "TutoringSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TutoringSessions_ModuleId",
                table: "TutoringSessions",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TutoringSessions_Modules_ModuleId",
                table: "TutoringSessions",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutoringSessions_Modules_ModuleId",
                table: "TutoringSessions");

            migrationBuilder.DropIndex(
                name: "IX_TutoringSessions_ModuleId",
                table: "TutoringSessions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "TutoringSessions");
        }
    }
}
