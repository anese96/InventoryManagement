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

        // Requires Microsoft.Extensions.DependencyInjection
        public void Open<T>(int id) where T : Form
        {
            var form = (T)ActivatorUtilities.CreateInstance(_provider, typeof(T), id);
            form.ShowDialog();
        }
    }
}
