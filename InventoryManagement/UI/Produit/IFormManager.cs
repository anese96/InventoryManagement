using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Produit
{
    public interface IFormManager
    {
        void Open<T>() where T : Form;
    }
}
