using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Logger;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using InventoryManagement.UI;
using InventoryManagement.UI.Client;
using InventoryManagement.UI.Fournisseur;
using InventoryManagement.UI.Produit;
using InventoryManagement.UI.Vente;
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

            services.AddScoped<AddEntityBD>();
            services.AddScoped<IRepository<ProduitDto>, ProduitRepository>();
            services.AddScoped<IService<ProduitDto>, ProduitService>();

            services.AddScoped<IRepository<ClientDto>, ClientRepository>();
            services.AddScoped<IService<ClientDto>, ClientService>();

            services.AddScoped<IRepository<VendorDto>,VendorRepository>();
            services.AddScoped<IService<VendorDto>, VendorService>();


            /// Ajouter -------------------------------------------------
            services.AddTransient<AjouterProduit>();
            services.AddTransient<AjouterClient>();
            services.AddTransient<AjouterFournisseur>();
            services.AddTransient<AjouterVente>();

            services.AddTransient<MainDashboard>(); 
            services.AddTransient<ModifierProduit>(); 
            services.AddTransient<ModifierClient>(); 

            services.AddSingleton<FunctionUI>();
            services.AddSingleton<IFormManager , FormFactory>();
            
         

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