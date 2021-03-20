using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class AddedModuleToChatMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ModuleId",
                table: "ChatMessages",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Modules_ModuleId",
                table: "ChatMessages",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Modules_ModuleId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ModuleId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "ChatMessages");
        }
    }
}
