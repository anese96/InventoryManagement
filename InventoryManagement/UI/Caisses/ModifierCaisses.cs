using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Caisses
{
    public partial class ModifierCaisses : Form
    {
        private readonly FunctionUI _functionUI;
        private TextBox  txtName, txtTotale;
        public ModifierCaisses()
        {
            InitializeComponent();
        }
    }
}
