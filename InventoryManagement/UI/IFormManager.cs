using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public interface IFormManager
    {
        void Open<T>() where T : Form;
        void Open<T>(int id ) where T : Form;
    }
}
