using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class addIdCaissePaymentVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCrates",
                table: "paymentVendors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_paymentVendors_IdCrates",
                table: "paymentVendors",
                column: "IdCrates");

            migrationBuilder.AddForeignKey(
                name: "FK_paymentVendors_Crates_IdCrates",
                table: "paymentVendors",
                column: "IdCrates",
                principalTable: "Crates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paymentVendors_Crates_IdCrates",
                table: "paymentVendors");

            migrationBuilder.DropIndex(
                name: "IX_paymentVendors_IdCrates",
                table: "paymentVendors");

            migrationBuilder.DropColumn(
                name: "IdCrates",
                table: "paymentVendors");
        }
    }
}
