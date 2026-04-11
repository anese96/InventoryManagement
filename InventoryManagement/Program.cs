using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using InventoryManagement.UI;
using InventoryManagement.UI.Produit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            var connectionString = ConfigurationManager
             .ConnectionStrings["InventoryDbConnection"]
             .ConnectionString;

            services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(connectionString));


            services.AddScoped<AddEntityBD>();
            services.AddScoped<IRepository<ProduitDto>, ProduitRepository>();
            services.AddScoped<IService<ProduitDto>, ProduitService>();

       

            services.AddTransient<AjouterProduit>(); // ﬂ· „—… ÃœÌœ
            services.AddTransient<MainDashboard>(); // ﬂ· „—… ÃœÌœ

            services.AddSingleton<FunctionUI>(); // Ê«Õœ ›ﬁÿ
            services.AddSingleton<IFormManager , FormFactory>(); //Ê«Õœ ›ﬁÿ

           


            ServiceProvider = services.BuildServiceProvider();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
           // Application.Run(new Form1());
            Application.Run(ServiceProvider.GetRequiredService<MainDashboard>());

            //Application.EnableVisualStyles();
            //Application.Run(new MainForm());

        }
    }
}