using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddNature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NatureId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_NatureId",
                table: "Products",
                column: "NatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Natures_NatureId",
                table: "Products",
                column: "NatureId",
                principalTable: "Natures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Natures_NatureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_NatureId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NatureId",
                table: "Products");
        }
    }
}
