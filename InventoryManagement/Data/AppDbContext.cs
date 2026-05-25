using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 //string connectionString = ConfigurationManager.ConnectionStrings["InventoryDbConnection"].ConnectionString;
                 //optionsBuilder.UseSqlServer(connectionString);
                 optionsBuilder.UseSqlite("Data Source=InventoryManagement.db");
            }
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Marque> Marques { get; set; }
        public DbSet<Nature> Natures { get; set; }
        public DbSet<PriceLists> PriceLists { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseLine> purchaseLines { get; set; }
        public DbSet<SalesInvoices> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceLine> salesInvoiceLines { get; set; }
        public DbSet<Crates> Crates { get; set; }
        public DbSet<CashTransaction> cashTransactions { get; set; }

        public DbSet<PaymentCustomer> paymentCustomers { get; set; }
        public DbSet<PaymentVendor>  paymentVendors { get; set; }
        public DbSet<ReturnPurchase> returnPurchases { get; set; }
        public DbSet<ReturnPurchaseLine> returnPurchaseLines { get; set; }
        public DbSet<ReturnSales>  returnSales { get; set; }
        public DbSet<ReturnSalesLine>  returnSalesLines { get; set; }

       
    }

}
