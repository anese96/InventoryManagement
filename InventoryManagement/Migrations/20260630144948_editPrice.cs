using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class editPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTaxRemise",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Remise",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "salesInvoiceLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "salesInvoiceLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "salesInvoiceLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "returnSalesLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "returnSalesLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReturn",
                table: "returnSales",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalInvoice",
                table: "returnSales",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GapTotal",
                table: "returnSales",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReturn",
                table: "returnPurchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchase",
                table: "returnPurchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GapTotal",
                table: "returnPurchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "returnPurchaseLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "returnPurchaseLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTaxRemise",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchase",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Remise",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentPurchase",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalancePurchase",
                table: "Purchases",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "purchaseLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "purchaseLines",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PriceLists",
                type: "decimal(18, 20)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "paymentVendors",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "paymentCustomers",
                type: "decimal(18, 20)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTaxRemise",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Remise",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceInvoice",
                table: "SalesInvoices",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "salesInvoiceLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "salesInvoiceLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "salesInvoiceLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "returnSalesLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "returnSalesLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReturn",
                table: "returnSales",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalInvoice",
                table: "returnSales",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GapTotal",
                table: "returnSales",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReturn",
                table: "returnPurchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchase",
                table: "returnPurchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GapTotal",
                table: "returnPurchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "returnPurchaseLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "returnPurchaseLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTaxRemise",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchase",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Remise",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentPurchase",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalancePurchase",
                table: "Purchases",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithoutTax",
                table: "purchaseLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "purchaseLines",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PriceLists",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "paymentVendors",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "paymentCustomers",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 20)");
        }
    }
}
