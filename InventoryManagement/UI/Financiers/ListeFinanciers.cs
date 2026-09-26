using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Financiers
{
    public partial class ListeFinanciers : Form
    {
        DataGridHelper helper;
        private readonly IFormManager _formFactory;
        private DataGridView dgvClients;
        private readonly AppDbContext _appContext;
        public ListeFinanciers(IFormManager formManager, AppDbContext appContext)
        {
            InitializeComponent();
            _formFactory = formManager;
            _appContext = appContext;
        }
    }
}
