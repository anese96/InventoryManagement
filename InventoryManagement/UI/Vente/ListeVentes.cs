using InventoryManagement.Data;
using InventoryManagement.UI.Produit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.UI.Vente
{
    public partial class ListeVentes : Form
    {
        DataGridHelper helper;
        private readonly IFormManager _formFactory;
        private readonly AppDbContext _appContext;
        private DataGridView dvgVentes;
        public ListeVentes(IFormManager formManager, AppDbContext appDbContext)
        {
            _formFactory = formManager;
            _appContext = appDbContext;
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Add header
            Label headerLabel = new Label
            {
                Text = "📦 Gestion des Ventes",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(headerLabel);

            // Add description
            Label descLabel = new Label
            {
                Text = "Gérez votre inventaire de produits ici. Ajoutez, modifiez ou supprimez des articles.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Location = new Point(30, 70)
            };
            this.Controls.Add(descLabel);

            // Add action panel
            Panel actionPanel = new Panel
            {
                Location = new Point(30, 120),
                Size = new Size(800, 60),
                BackColor = Color.White
            };

            System.Windows.Forms.Button btnAddProduct = new System.Windows.Forms.Button
            {
                Text = "➕ Ajouter un Vente",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(10, 8),
                Cursor = Cursors.Hand
            };
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.Click += (s, e) =>
            {
                _formFactory.Open<AjouterVente>();
                LoadData();
            };
            actionPanel.Controls.Add(btnAddProduct);

            this.Controls.Add(actionPanel);

            //------------------------------------------------------
            helper = new DataGridHelper(this);
            dvgVentes = helper.Grid;
            dvgVentes.CellDoubleClick += Grid_CellDoubleClick;

            LoadData();
        }

        private void LoadData()
        {
          
            var salesInvoices = _appContext.SalesInvoices.Select(v => new
            {
               ID = v.Id,
                N_Facture = v.NumberInvoice,
                Date = v.DateInvoice,
               Client=v.Customer.Name,
                Total_HT = v.TotalWithoutTax,
                Remise = v.Remise,
                Total_TTC = v.TotalInvoice,
                Montant_Payé = v.PaymentInvoice
            }).OrderByDescending(v => v.Date).ToList();

            helper.SetData(salesInvoices);
            if (dvgVentes.Columns["ID"] != null) dvgVentes.Columns["ID"].Visible = false;

        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dvgVentes.SelectedRows.Count == 0) return;
                var row = dvgVentes.SelectedRows[0];
                if (row.Cells["ID"].Value == null) return;
                int id = Convert.ToInt32(row.Cells["ID"].Value);
                _formFactory.Open<ModifierVente>(id);
                LoadData();
            }
        }
    }
}
