using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizGen.DAL.Migrations
{
    public partial class RemoveCorrectColumnFromQuestionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correct",
                table: "Questions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Correct",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
