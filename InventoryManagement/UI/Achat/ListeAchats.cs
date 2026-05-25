using InventoryManagement.Data;
using InventoryManagement.UI.Vente;
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

namespace InventoryManagement.UI.Achat
{
    public partial class ListeAchats : Form
    {
        private readonly IFormManager _formFactory;
        private readonly AppDbContext _appDbContext;
        DataGridHelper helper;
        private DataGridView dvgVentes;
        private AppDbContext appContext;

        public ListeAchats(IFormManager formFactory, AppDbContext appDbContext)
        {
            _formFactory = formFactory;
            _appDbContext = appDbContext;
            InitializeComponent();
            InitializeCustomComponents();
        }      

        private void InitializeCustomComponents()
        {
            // Add header
            Label headerLabel = new Label
            {
                Text = "📦 Gestion des Achats",
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
                Text = "➕ Ajouter un Achat",
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
                _formFactory.Open<AjouterAchat>();
               // LoadData();
            };
            actionPanel.Controls.Add(btnAddProduct);

            this.Controls.Add(actionPanel);

            //------------------------------------------------------
            helper = new DataGridHelper(this);
            dvgVentes = helper.Grid;
          //  dvgVentes.CellDoubleClick += Grid_CellDoubleClick;

           // LoadData();
        }
    }
}
