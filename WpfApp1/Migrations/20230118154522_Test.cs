using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_GameCategories_GameCategoryGameID_GameCategoryCategoryID",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_GameCategoryGameID_GameCategoryCategoryID",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "GameCategoryGameID",
                table: "Results",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "GameCategoryCategoryID",
                table: "Results",
                newName: "GameID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                columns: new[] { "PlayerID", "GameID", "CategoryID" });

            migrationBuilder.CreateIndex(
                name: "IX_Results_CategoryID",
                table: "Results",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Results_GameID_CategoryID",
                table: "Results",
                columns: new[] { "GameID", "CategoryID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Categories_CategoryID",
                table: "Results",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_GameCategories_GameID_CategoryID",
                table: "Results",
                columns: new[] { "GameID", "CategoryID" },
                principalTable: "GameCategories",
                principalColumns: new[] { "GameID", "CategoryID" },
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Games_GameID",
                table: "Results",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Categories_CategoryID",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_GameCategories_GameID_CategoryID",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Games_GameID",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_CategoryID",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_GameID_CategoryID",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Results",
                newName: "GameCategoryGameID");

            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "Results",
                newName: "GameCategoryCategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                columns: new[] { "PlayerID", "GameCategoryID" });

            migrationBuilder.CreateIndex(
                name: "IX_Results_GameCategoryGameID_GameCategoryCategoryID",
                table: "Results",
                columns: new[] { "GameCategoryGameID", "GameCategoryCategoryID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Results_GameCategories_GameCategoryGameID_GameCategoryCategoryID",
                table: "Results",
                columns: new[] { "GameCategoryGameID", "GameCategoryCategoryID" },
                principalTable: "GameCategories",
                principalColumns: new[] { "GameID", "CategoryID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
