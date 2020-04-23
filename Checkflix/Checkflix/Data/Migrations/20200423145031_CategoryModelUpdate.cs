using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkflix.Data.Migrations
{
    public partial class CategoryModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreApiId",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreApiId",
                table: "Categories");
        }
    }
}
