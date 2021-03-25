using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class RemovedTutorEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TutorEvaluations");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "TutoringSessions",
                newName: "CreationDate");

            migrationBuilder.AddColumn<int>(
                name: "Evaluation",
                table: "TutoringSessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationComment",
                table: "TutoringSessions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "TutoringSessions");

            migrationBuilder.DropColumn(
                name: "EvaluationComment",
                table: "TutoringSessions");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "TutoringSessions",
                newName: "RequestDate");

            migrationBuilder.CreateTable(
                name: "TutorEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Evaluation = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TutorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorEvaluations_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorEvaluations_AspNetUsers_TutorId",
                        column: x => x.TutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TutorEvaluations_StudentId",
                table: "TutorEvaluations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorEvaluations_TutorId",
                table: "TutorEvaluations",
                column: "TutorId");
        }
    }
}
