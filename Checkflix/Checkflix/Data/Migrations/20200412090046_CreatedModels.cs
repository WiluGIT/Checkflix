using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkflix.Data.Migrations
{
    public partial class CreatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Productions",
                columns: table => new
                {
                    ProductionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Synopsis = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    ImbdId = table.Column<int>(nullable: false),
                    ImbdRating = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productions", x => x.ProductionId);
                });

            migrationBuilder.CreateTable(
                name: "Vods",
                columns: table => new
                {
                    VodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vods", x => x.VodId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserCategories",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCategories", x => new { x.ApplicationUserId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCategories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserProductions",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    ProductionId = table.Column<int>(nullable: false),
                    Favourites = table.Column<bool>(nullable: false),
                    ToWatch = table.Column<bool>(nullable: false),
                    Watched = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProductions", x => new { x.ApplicationUserId, x.ProductionId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProductions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProductions_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "ProductionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionCategories",
                columns: table => new
                {
                    ProductionId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCategories", x => new { x.ProductionId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductionCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionCategories_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "ProductionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserVods",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    VodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserVods", x => new { x.ApplicationUserId, x.VodId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserVods_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserVods_Vods_VodId",
                        column: x => x.VodId,
                        principalTable: "Vods",
                        principalColumn: "VodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VodProductions",
                columns: table => new
                {
                    VodId = table.Column<int>(nullable: false),
                    ProductionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodProductions", x => new { x.VodId, x.ProductionId });
                    table.ForeignKey(
                        name: "FK_VodProductions_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "ProductionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VodProductions_Vods_VodId",
                        column: x => x.VodId,
                        principalTable: "Vods",
                        principalColumn: "VodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCategories_CategoryId",
                table: "ApplicationUserCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProductions_ProductionId",
                table: "ApplicationUserProductions",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserVods_VodId",
                table: "ApplicationUserVods",
                column: "VodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCategories_CategoryId",
                table: "ProductionCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VodProductions_ProductionId",
                table: "VodProductions",
                column: "ProductionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCategories");

            migrationBuilder.DropTable(
                name: "ApplicationUserProductions");

            migrationBuilder.DropTable(
                name: "ApplicationUserVods");

            migrationBuilder.DropTable(
                name: "ProductionCategories");

            migrationBuilder.DropTable(
                name: "VodProductions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Productions");

            migrationBuilder.DropTable(
                name: "Vods");
        }
    }
}
