using InventoryManagement.Data;
using InventoryManagement.Data.Models;
using InventoryManagement.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InventoryManagement.UI.Produit
{
    public partial class ListeProduits : Form
    {
        DataGridHelper helper;
        private readonly IFormManager _formFactory;
        private DataGridView dgvProducts;
        
        public ListeProduits(IFormManager formFactory)
        {
            _formFactory = formFactory;
            InitializeComponent();
            InitializeCustomComponents();
        }

        public void InitializeCustomComponents()
        {

            // Add header
            Label headerLabel = new Label
            {
                Text = "📦 Gestion des Produits",
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
                Text = "➕ Ajouter un Produit",
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
                _formFactory.Open<AjouterProduit>();
                LoadData();
            };
            actionPanel.Controls.Add(btnAddProduct);

            this.Controls.Add(actionPanel);

            //------------------------------------------------------
            helper = new DataGridHelper(this);
            dgvProducts = helper.Grid;
            dgvProducts.CellDoubleClick += Grid_CellDoubleClick;

            LoadData();

        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvProducts.SelectedRows.Count == 0) return;
                var row = dgvProducts.SelectedRows[0];
                if (row.Cells["ID"].Value == null) return;
                int id = Convert.ToInt32(row.Cells["ID"].Value);             
                 _formFactory.Open<ModifierProduit>(id);
                 LoadData();
            }

        }

        private void LoadData()
        {
            var db = new AppDbContext();
            var produits = db.Products.Select(p => new
            {
                ID = p.Id,
                Référence = p.RefProduct,
                Désignation = p.Designation,
                Code_Bare=p.BarCode,
                Stock = (int)(p.StockQuantity ?? 0),
                Prix_Achat = p.PurchasePrice,
                Prix_Vente = p.SalesPrice,
                Catégorie = p.Category.Name,
                Marque = p.Marque.Name,
                Nature= p.Nature.Name,
                Unité=p.Unit.Name,
                Colisage=p.Colisage,
            }).ToList();

            helper.SetData(produits);
            if (dgvProducts.Columns["ID"] != null) dgvProducts.Columns["ID"].Visible = false;
            if (dgvProducts.Columns["Nature"] != null) dgvProducts.Columns["Nature"].Visible = false;
            if (dgvProducts.Columns["Code_Bare"] != null) dgvProducts.Columns["Code_Bare"].Visible = false;
            if (dgvProducts.Columns["Unité"] != null) dgvProducts.Columns["Unité"].Visible = false;
            if (dgvProducts.Columns["Colisage"] != null) dgvProducts.Columns["Colisage"].Visible = false;
        }
    }
}
