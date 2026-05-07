using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoiceAndPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RefVendor = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Remark = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: true),
                    Turnover = table.Column<decimal>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeleterId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberInvoice = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateInvoice = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdCustomer = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Remise = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalWithoutTaxRemise = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalInvoice = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PaymentInvoice = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    BalanceInvoice = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    IdCrates = table.Column<int>(type: "INTEGER", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeleterId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberPurchase = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DatePurchase = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdVendor = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Remise = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalWithoutTaxRemise = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalPurchase = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PaymentPurchase = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    BalancePurchase = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeleterId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Vendors_IdVendor",
                        column: x => x.IdVendor,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "salesInvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSalesInvoice = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxe = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesInvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salesInvoiceLines_SalesInvoices_IdSalesInvoice",
                        column: x => x.IdSalesInvoice,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchaseLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPurchase = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxe = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchaseLines_Purchases_IdPurchase",
                        column: x => x.IdPurchase,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchaseLines_IdPurchase",
                table: "purchaseLines",
                column: "IdPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_IdVendor",
                table: "Purchases",
                column: "IdVendor");

            migrationBuilder.CreateIndex(
                name: "IX_salesInvoiceLines_IdSalesInvoice",
                table: "salesInvoiceLines",
                column: "IdSalesInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_IdCrates",
                table: "SalesInvoices",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_IdCustomer",
                table: "SalesInvoices",
                column: "IdCustomer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchaseLines");

            migrationBuilder.DropTable(
                name: "salesInvoiceLines");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Crates");
        }
    }
}
