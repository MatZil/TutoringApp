using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringApp.Migrations
{
    public partial class AddedSubmissionsToAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Assignments");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentFileName",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmissionEvaluation",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubmissionFileName",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionEvaluation",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "SubmissionFileName",
                table: "Assignments");

            migrationBuilder.AlterColumn<string>(
                name: "AssignmentFileName",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationDate",
                table: "Assignments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
