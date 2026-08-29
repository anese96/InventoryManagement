using InventoryManagement.Data;
using InventoryManagement.UI.Client;
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
    public partial class ListeCaisses : Form
    {
        DataGridHelper helper;
        private readonly IFormManager _formFactory;
        private DataGridView dgvCaisses;
        private readonly AppDbContext _appContext;
        public ListeCaisses(IFormManager formManager, AppDbContext appContext)
        {
            InitializeComponent();
            _formFactory = formManager;
            _appContext = appContext;
            InitializeCustomComponents();
        }
        private void InitializeCustomComponents()
        {
            // Add header
            Label headerLabel = new Label
            {
                Text = "📦 Gestion des Caisses",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(headerLabel);

            // Add description
            Label descLabel = new Label
            {
                Text = "Gérez votre inventaire de Caisses ici. ",
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

            //System.Windows.Forms.Button btnAddProduct = new System.Windows.Forms.Button
            //{
            //    Text = "➕ Ajouter un Client",
            //    Font = new Font("Segoe UI", 11, FontStyle.Bold),
            //    BackColor = Color.FromArgb(52, 152, 219),
            //    ForeColor = Color.White,
            //    FlatStyle = FlatStyle.Flat,
            //    Size = new Size(200, 45),
            //    Location = new Point(10, 8),
            //    Cursor = Cursors.Hand
            //};
            //btnAddProduct.FlatAppearance.BorderSize = 0;
            //btnAddProduct.Click += (s, e) =>
            //{
            //    _formFactory.Open<AjouterClient>();
            //    LoadData();
            //};
            //actionPanel.Controls.Add(btnAddProduct);

            this.Controls.Add(actionPanel);

            //------------------------------------------------------
            helper = new DataGridHelper(this);
            dgvCaisses = helper.Grid;
          //  dgvCaisses.CellDoubleClick += Grid_CellDoubleClick;

            LoadData();
        }
        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    if (dgvCaisses.SelectedRows.Count == 0) return;
            //    var row = dgvCaisses.SelectedRows[0];
            //    if (row.Cells["ID"].Value == null) return;
            //    int id = Convert.ToInt32(row.Cells["ID"].Value);
            //    _formFactory.Open<ModifierClient>(id);
            //    LoadData();
            //}
        }
        private void LoadData()
        {

            var caisses = _appContext.Crates.Select(p => new
            {
                ID = p.Id,
              
                Nom = p.Name,
                Total = p.Totale
            }).ToList();
            helper.SetData(caisses);
            if (dgvCaisses.Columns["ID"] != null) dgvCaisses.Columns["ID"].Visible = false;

        }
    }
}
