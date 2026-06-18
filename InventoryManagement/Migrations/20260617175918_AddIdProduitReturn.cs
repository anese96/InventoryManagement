using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddIdProduitReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProduct",
                table: "returnSalesLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProduct",
                table: "returnPurchaseLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_returnSalesLines_IdProduct",
                table: "returnSalesLines",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_returnPurchaseLines_IdProduct",
                table: "returnPurchaseLines",
                column: "IdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_returnPurchaseLines_Products_IdProduct",
                table: "returnPurchaseLines",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_returnSalesLines_Products_IdProduct",
                table: "returnSalesLines",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_returnPurchaseLines_Products_IdProduct",
                table: "returnPurchaseLines");

            migrationBuilder.DropForeignKey(
                name: "FK_returnSalesLines_Products_IdProduct",
                table: "returnSalesLines");

            migrationBuilder.DropIndex(
                name: "IX_returnSalesLines_IdProduct",
                table: "returnSalesLines");

            migrationBuilder.DropIndex(
                name: "IX_returnPurchaseLines_IdProduct",
                table: "returnPurchaseLines");

            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "returnSalesLines");

            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "returnPurchaseLines");
        }
    }
}
