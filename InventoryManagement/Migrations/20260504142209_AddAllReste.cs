using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddAllReste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cashTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCrates = table.Column<int>(type: "INTEGER", nullable: true),
                    DateCash = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
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
                    table.PrimaryKey("PK_cashTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cashTransactions_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "paymentCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCustomer = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberPayment = table.Column<string>(type: "TEXT", nullable: false),
                    DatePayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
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
                    table.PrimaryKey("PK_paymentCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paymentCustomers_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paymentVendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdVendor = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberPayment = table.Column<string>(type: "TEXT", nullable: false),
                    DatePayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
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
                    table.PrimaryKey("PK_paymentVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paymentVendors_Vendors_IdVendor",
                        column: x => x.IdVendor,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "returnPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberReturn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateReturn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdVendor = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalPurchase = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalReturn = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    GapTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    IdCrates = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_returnPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_returnPurchases_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_returnPurchases_Vendors_IdVendor",
                        column: x => x.IdVendor,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "returnSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberReturn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateReturn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdCustomer = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalInvoice = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TotalReturn = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    GapTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    IdCrates = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_returnSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_returnSales_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_returnSales_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "returnPurchaseLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdReturnSales = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxe = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_returnPurchaseLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_returnPurchaseLines_returnPurchases_IdReturnSales",
                        column: x => x.IdReturnSales,
                        principalTable: "returnPurchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "returnSalesLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdReturnSales = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxe = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_returnSalesLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_returnSalesLines_returnSales_IdReturnSales",
                        column: x => x.IdReturnSales,
                        principalTable: "returnSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cashTransactions_IdCrates",
                table: "cashTransactions",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_paymentCustomers_IdCustomer",
                table: "paymentCustomers",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_paymentVendors_IdVendor",
                table: "paymentVendors",
                column: "IdVendor");

            migrationBuilder.CreateIndex(
                name: "IX_returnPurchaseLines_IdReturnSales",
                table: "returnPurchaseLines",
                column: "IdReturnSales");

            migrationBuilder.CreateIndex(
                name: "IX_returnPurchases_IdCrates",
                table: "returnPurchases",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_returnPurchases_IdVendor",
                table: "returnPurchases",
                column: "IdVendor");

            migrationBuilder.CreateIndex(
                name: "IX_returnSales_IdCrates",
                table: "returnSales",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_returnSales_IdCustomer",
                table: "returnSales",
                column: "IdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_returnSalesLines_IdReturnSales",
                table: "returnSalesLines",
                column: "IdReturnSales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cashTransactions");

            migrationBuilder.DropTable(
                name: "paymentCustomers");

            migrationBuilder.DropTable(
                name: "paymentVendors");

            migrationBuilder.DropTable(
                name: "returnPurchaseLines");

            migrationBuilder.DropTable(
                name: "returnSalesLines");

            migrationBuilder.DropTable(
                name: "returnPurchases");

            migrationBuilder.DropTable(
                name: "returnSales");
        }
    }
}
