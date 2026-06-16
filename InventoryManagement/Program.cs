using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Logger;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using InventoryManagement.UI;
using InventoryManagement.UI.Achat;
using InventoryManagement.UI.Client;
using InventoryManagement.UI.Fournisseur;
using InventoryManagement.UI.Produit;
using InventoryManagement.UI.Vente;
using InventoryManagement.UI.VentesComptoir;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Windows.Forms.Design;

namespace InventoryManagement
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
          public static ServiceProvider ServiceProvider;

        [STAThread]
        static void Main()
        {
           
            var services = new ServiceCollection();

            //var connectionString = ConfigurationManager
            // .ConnectionStrings["InventoryDbConnection"]
            // .ConnectionString;

            //services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(connectionString));
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=InventoryManagement.db"));

            services.AddTransient<AddEntityBD>();
            services.AddTransient<IRepository<ProduitDto>, ProduitRepository>();
            services.AddTransient<IService<ProduitDto>, ProduitService>();

            services.AddTransient<IRepository<ClientDto>, ClientRepository>();
            services.AddTransient<IService<ClientDto>, ClientService>();

            services.AddTransient<IRepository<VendorDto>,VendorRepository>();
            services.AddTransient<IService<VendorDto>, VendorService>();

            services.AddTransient<IRepository<SalesInvoicesDto>,SalesInvoicesRepository>();
            services.AddTransient<IService<SalesInvoicesDto>, SalesInvoicesService>();

            services.AddTransient<IRepository<SalesInvoiceLineDto>,SalesInvoiceLineRepository>();
            services.AddTransient<IService<SalesInvoiceLineDto>, SalesInvoiceLineService>();

            services.AddTransient<IRepository<PurchaseDto>,PurchaseRepository>();
            services.AddTransient<IService<PurchaseDto>, PurchaseService>();

            services.AddTransient<IRepository<PurchaseLineDto>,PurchaseLineRepository>();
            services.AddTransient<IService<PurchaseLineDto>, PurchaseLineService>();

            services.AddTransient<IRepository<PaymentCustomerDto>,PaymentCustomerRepository>();
            services.AddTransient<IService<PaymentCustomerDto>, PaymentCustomerService>();

            services.AddTransient<IRepository<PaymentVendorDto>,PaymentVendorRepository>();
            services.AddTransient<IService<PaymentVendorDto>, PaymentVendorService>();

   

           


            /// Ajouter  -------------------------------------------------
            services.AddTransient<AjouterProduit>();
            services.AddTransient<AjouterClient>();
            services.AddTransient<AjouterFournisseur>();
            services.AddTransient<AjouterVente>();
            services.AddTransient<AjouterPayment>();
            services.AddTransient<AjouterAchat>();
            services.AddTransient<AjouterPaymentFournisseur>();
            services.AddTransient<AjouterVentesComptoir>();
            services.AddTransient<Qte__Prix>();
            services.AddTransient<AjouterListeProduit>();
           

            /// Modifier ----------------------------------------------------
            services.AddTransient<ModifierProduit>(); 
            services.AddTransient<ModifierClient>(); 
            services.AddTransient<ModifierVente>();
            services.AddTransient<ModifierAchat>();
            services.AddTransient<ModifierFournisseur>();


           
            services.AddTransient<GetTotal>();
            services.AddTransient<MainDashboard>();
            services.AddSingleton<FunctionUI>();
            services.AddTransient<ProduitRepository>();
            services.AddTransient<ClientService>();           
            services.AddTransient<ClientRepository>();
            services.AddTransient<VendorService>();
            services.AddTransient<VendorRepository>();
            services.AddTransient<IFormManager , FormFactory>();
            
         

            Application.ThreadException += static (sender, e) =>
            {
                ErrorLogger.Log("Erreur UI: " + e.Exception.ToString());
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                ErrorLogger.Log("Erreur critique: " + e.ExceptionObject.ToString());
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceProvider = services.BuildServiceProvider();  
            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<MainDashboard>());


        }
    }
}