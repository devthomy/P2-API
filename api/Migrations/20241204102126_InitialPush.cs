using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D1_P2_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialPush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CorrectAnswer",
                table: "Defis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectOptionNumber",
                table: "Defis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option1",
                table: "Defis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option2",
                table: "Defis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option3",
                table: "Defis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option4",
                table: "Defis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                table: "Defis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Defis",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "CorrectOptionNumber",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "Option1",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "Option3",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "Option4",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                table: "Defis");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Defis");
        }
    }
}
