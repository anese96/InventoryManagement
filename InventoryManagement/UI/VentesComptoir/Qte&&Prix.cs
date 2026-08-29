using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.VentesComptoir
{
    public partial class Qte__Prix : Form
    {
      
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appDbContext;
        public int IdProduit;
        public TextBox Qte , Qte_pack, Prix;
        public ComboBox Tarification;
        private decimal? _colisage;
        private bool _isUpdatingQty = false;

        public Qte__Prix( FunctionUI functionUI, AppDbContext appDbContext )
        {
            
            _functionUI = functionUI;
            _appDbContext = appDbContext;
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Prix && Qté";
            this.Size = new Size(700, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.Controls.Add(mainLayout);

            // Header
            Label lblHeader = new Label
            {
                Text = " Prix && Qté",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.Controls.Add(lblHeader, 0, 0);

            // Content Layout
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1,
            };
            mainLayout.Controls.Add(contentLayout, 0, 1);

            FlowLayoutPanel formPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(20, 10, 20, 10)
            };
            contentLayout.Controls.Add(formPanel, 0, 0);

            int lblW = 150, fldW = 450, spc = 10;

            _functionUI.AddFormField(formPanel, "Qte :", lblW, fldW, 30, spc, out Qte);
            _functionUI.AddFormField(formPanel, "Qte Pack :", lblW, fldW, 30, spc, out Qte_pack);
            _functionUI.AddFormField(formPanel, "Prix :", lblW, fldW, 30, spc, out Prix);
       
            _functionUI.AddComboBoxField(formPanel, "Tarification:", lblW, fldW, spc, out Tarification);

            // Wire event handlers
            Tarification.SelectedIndexChanged += Tarification_SelectedIndexChanged;
            Qte.TextChanged += Qte_TextChanged;
            Qte_pack.TextChanged += Qte_pack_TextChanged;

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 5, 25, 5),
                BackColor = Color.FromArgb(236, 240, 241)
            };
            mainLayout.Controls.Add(buttonPanel, 0, 2);

            Button btnCancel = new Button
            {
                Text = "❌ Annuler",
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += _functionUI.BtnCancel_Click;

            Button btnSave = new Button
            {
                Text = "💾 OK ",
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        public void LoadProductData(Product product)
        {
            this.IdProduit = product.Id;
            this._colisage = product.Colisage;

            // Set values without triggering text changed loops
            _isUpdatingQty = true;
            this.Qte.Text = "1";
            if (_colisage.HasValue && _colisage.Value > 0)
            {
                this.Qte_pack.Text = (1.0m / _colisage.Value).ToString("0");
            }
            else
            {
                this.Qte_pack.Text = "0";
            }
            this.Prix.Text = product.SalesPrice?.ToString("F6") ?? "0.000000";
            _isUpdatingQty = false;

            // Populate Tarification options
            try
            {
                using (var db = new AppDbContext())
                {
                    var priceLists = db.PriceLists.Where(pl => pl.ProductId == product.Id).ToList();
                    var options = new List<PriceOption>();
                    options.Add(new PriceOption { Display = $"Standard ({product.SalesPrice:N2})", Value = product.SalesPrice });
                    foreach (var pl in priceLists)
                    {
                        options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N2})", Value = pl.Price });
                    }

                    Tarification.DataSource = options;
                    Tarification.DisplayMember = "Display";
                    Tarification.ValueMember = "Value";
                    Tarification.SelectedValue = product.SalesPrice;
                }
            }
            catch { }
        }

        private void Tarification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tarification.SelectedValue is decimal p)
            {
                Prix.Text = p.ToString("F2");
            }
            else if (Tarification.SelectedValue != null && decimal.TryParse(Tarification.SelectedValue.ToString(), out decimal decVal))
            {
                Prix.Text = decVal.ToString("F2");
            }
        }

        private void Qte_pack_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingQty) return;
            if (_colisage.HasValue && _colisage.Value > 0)
            {
                if (decimal.TryParse(Qte_pack.Text, out decimal qtePack))
                {
                    _isUpdatingQty = true;
                    Qte.Text = (qtePack * _colisage.Value).ToString("G29");
                    _isUpdatingQty = false;
                }
            }
        }

        private void Qte_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingQty) return;
            if (_colisage.HasValue && _colisage.Value > 0)
            {
                if (decimal.TryParse(Qte.Text, out decimal qte))
                {
                    _isUpdatingQty = true;
                    Qte_pack.Text = (qte / _colisage.Value).ToString("G29");
                    _isUpdatingQty = false;
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!decimal.TryParse(Qte.Text, out decimal qte) || qte <= 0)
            {
                MessageBox.Show("Veuillez saisir une quantité valide supérieure à 0.", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Qte.Focus();
                return;
            }

            if (!decimal.TryParse(Prix.Text, out decimal prix) || prix < 0)
            {
                MessageBox.Show("Veuillez saisir un prix valide.", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Prix.Focus();
                return;
            }
            var product = _appDbContext.Products
             .FirstOrDefault(x => x.Id == IdProduit);
            if (product.StockQuantity < Convert.ToDecimal(Qte.Text))
            {
                MessageBox.Show("Quantité insuffisante en stock.", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Qte.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public class PriceOption
        {
            public string Display { get; set; }
            public decimal? Value { get; set; }
        }
    }
}
