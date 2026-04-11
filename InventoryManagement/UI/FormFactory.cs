using InventoryManagement.UI.Produit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public class FormFactory : IFormManager
    {
        private readonly IServiceProvider _provider;

        public FormFactory( IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }
       

        public void Open<T>() where T : Form
        {
            var form = _provider.GetRequiredService<T>();
            form.ShowDialog();
        }
    }
}
