using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Migrations
{
    /// <inheritdoc />
    public partial class Firdt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RefCustomer = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Natures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
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
                    IdCrates = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_paymentCustomers_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentCustomers_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Taxe = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    BarCode = table.Column<string>(type: "TEXT", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "TEXT", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "TEXT", nullable: true),
                    StockQuantity = table.Column<decimal>(type: "TEXT", nullable: true),
                    QtyAlert = table.Column<decimal>(type: "TEXT", nullable: true),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    Colisage = table.Column<int>(type: "INTEGER", nullable: true),
                    MarqueId = table.Column<int>(type: "INTEGER", nullable: true),
                    NatureId = table.Column<int>(type: "INTEGER", nullable: true),
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Marques_MarqueId",
                        column: x => x.MarqueId,
                        principalTable: "Marques",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Natures_NatureId",
                        column: x => x.NatureId,
                        principalTable: "Natures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
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
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Crates_IdCrates",
                        column: x => x.IdCrates,
                        principalTable: "Crates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchases_Vendors_IdVendor",
                        column: x => x.IdVendor,
                        principalTable: "Vendors",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salesInvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSalesInvoice = table.Column<int>(type: "INTEGER", nullable: false),
                    IdProduct = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Taxe = table.Column<string>(type: "TEXT", nullable: false),
                    TotalWithoutTax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesInvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salesInvoiceLines_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    IdProduct = table.Column<int>(type: "INTEGER", nullable: false),
                    RefProduct = table.Column<string>(type: "TEXT", nullable: false),
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
                        name: "FK_purchaseLines_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchaseLines_Purchases_IdPurchase",
                        column: x => x.IdPurchase,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_cashTransactions_IdCrates",
                table: "cashTransactions",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_paymentCustomers_IdCrates",
                table: "paymentCustomers",
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
                name: "IX_PriceLists_ProductId",
                table: "PriceLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MarqueId",
                table: "Products",
                column: "MarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NatureId",
                table: "Products",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseLines_IdProduct",
                table: "purchaseLines",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseLines_IdPurchase",
                table: "purchaseLines",
                column: "IdPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_IdCrates",
                table: "Purchases",
                column: "IdCrates");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_IdVendor",
                table: "Purchases",
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

            migrationBuilder.CreateIndex(
                name: "IX_salesInvoiceLines_IdProduct",
                table: "salesInvoiceLines",
                column: "IdProduct");

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
                name: "cashTransactions");

            migrationBuilder.DropTable(
                name: "paymentCustomers");

            migrationBuilder.DropTable(
                name: "paymentVendors");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "purchaseLines");

            migrationBuilder.DropTable(
                name: "returnPurchaseLines");

            migrationBuilder.DropTable(
                name: "returnSalesLines");

            migrationBuilder.DropTable(
                name: "salesInvoiceLines");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "returnPurchases");

            migrationBuilder.DropTable(
                name: "returnSales");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Marques");

            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Crates");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
