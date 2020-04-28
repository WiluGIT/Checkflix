using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkflix.Data.Migrations
{
    public partial class AddedCategoriesForTVShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "GenreApiId" },
                values: new object[,]
                {
                    { 20, "Akcja i Przygoda", 10759 },
                    { 21, "Dzieci", 10762 },
                    { 22, "News", 10763 },
                    { 23, "Reality", 10764 },
                    { 24, "Sci-Fi & Fantasy", 10765 },
                    { 25, "Opera", 10766 },
                    { 26, "Rozmowy", 10767 },
                    { 27, "Wojna & Polityka", 10768 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 27);
        }
    }
}
