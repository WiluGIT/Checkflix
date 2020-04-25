using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkflix.Data.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "GenreApiId" },
                values: new object[,]
                {
                    { 1, "Akcja", 28 },
                    { 19, "Western", 37 },
                    { 18, "Fantasy", 14 },
                    { 17, "Przygodowy", 12 },
                    { 16, "Animacja", 16 },
                    { 15, "Komedia", 35 },
                    { 14, "Kryminał", 80 },
                    { 13, "Dokumentalny", 99 },
                    { 12, "Dramat", 18 },
                    { 11, "Familijny", 10751 },
                    { 9, "Wojenny", 10752 },
                    { 8, "Horror", 27 },
                    { 7, "Muzyczny", 10402 },
                    { 6, "Tajemnica", 9648 },
                    { 5, "Romans", 10749 },
                    { 4, "Sci-Fi", 878 },
                    { 3, "film TV", 10770 },
                    { 2, "Thriller", 53 },
                    { 10, "Historyczny", 36 }
                });

            migrationBuilder.InsertData(
                table: "Vods",
                columns: new[] { "VodId", "PlatformName" },
                values: new object[,]
                {
                    { 1, "Netflix" },
                    { 2, "HBO GO" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Vods",
                keyColumn: "VodId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vods",
                keyColumn: "VodId",
                keyValue: 2);
        }
    }
}
