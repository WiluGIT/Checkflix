using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkflix.Data.Migrations
{
    public partial class SubtitleAddToProduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "Productions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "Productions");
        }
    }
}
