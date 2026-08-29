using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Produit
{
    public partial class AjouterProduit : Form
    {

        private readonly IService<ProduitDto> _service;
        //private readonly ProduitService _duitService;

        public TextBox txtRef, txtDesignation, txtBarCode, txtPurchasePrice, txtSalesPrice, txtStock, txtAlertQty, txtColisage;
        public DataGridView dgvPriceLists;
        public ComboBox cbCategory, cbTaxe, cbUnit, cbNature, cbBrand , cbIsFavorite;
        public CheckBox cbAutoBarecode;
        
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appContext;

        public AjouterProduit(FunctionUI functionUI , IService<ProduitDto> service , AppDbContext appContext)
        {
            _functionUI = functionUI;
            _service = service;
            _appContext = appContext;
            InitializeComponent();
            InitializeCustomComponents();
            LoadAllData();
        }
        private void LoadAllData()
        {
            try
            {
                    cbCategory.DataSource = _appContext.Categories.OrderBy(c => c.Name).ToList();
                    cbCategory.DisplayMember = "Name";
                    cbCategory.ValueMember = "Id";
                    cbCategory.SelectedIndex = -1;

                    cbUnit.DataSource = _appContext.Units.OrderBy(c => c.Name).ToList();
                    cbUnit.DisplayMember = "Name";
                    cbUnit.ValueMember = "Id";
                    cbUnit.SelectedIndex = -1;

                    cbBrand.DataSource = _appContext.Marques.OrderBy(c => c.Name).ToList();
                    cbBrand.DisplayMember = "Name";
                    cbBrand.ValueMember = "Id";
                    cbBrand.SelectedIndex = -1;

                    cbNature.DataSource = _appContext.Natures.OrderBy(c => c.Name).ToList();
                    cbNature.DisplayMember = "Name";
                    cbNature.ValueMember = "Id";
                    cbNature.SelectedIndex = -1;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de chargement : " + ex.Message);
            }
        }
        public void InitializeCustomComponents()
        {
            this.Text = "Ajouter un Produit";
            this.Size = new Size(1250, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));  // Header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Fields
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));  // Buttons
            this.Controls.Add(mainLayout);
            Label lblHeader = new Label
            {
                Text = "📦  Produit",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(44, 62, 80),

                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainLayout.Controls.Add(lblHeader, 0, 0);
            TableLayoutPanel bodyLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(20, 10, 20, 10)
            };
            bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainLayout.Controls.Add(bodyLayout, 0, 1);

            FlowLayoutPanel leftPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            FlowLayoutPanel rightPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            bodyLayout.Controls.Add(leftPanel, 0, 0);
            bodyLayout.Controls.Add(rightPanel, 1, 0);

            int lblW = 120, fldW = 380, spc = 10;
            _functionUI.AddFormField(leftPanel, "Référence:", lblW, fldW, 30, spc, out txtRef, "", "", false);
            txtRef.Text = "PRO-" + DateTime.Now.ToString("HHmmss");
            _functionUI.AddFormField(leftPanel, "Désignation:", lblW, fldW, 30, spc, out txtDesignation, "", "", false);
            _functionUI.AddFormField(leftPanel, "Code Barre:", lblW, fldW, 30, spc, out txtBarCode, "", "", false);

            // Configure cbAutoBarecode and place it next to txtBarCode
            Panel barcodePanel = leftPanel.Controls[leftPanel.Controls.Count - 1] as Panel;
            if (barcodePanel != null)
            {
                txtBarCode.Width -= 100; // Reduce width to fit checkbox
                cbAutoBarecode = new CheckBox
                {
                    Text = "Auto",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9),
                    Location = new Point(txtBarCode.Right + 10, txtBarCode.Top + 5),
                    Cursor = Cursors.Hand,
                    TabStop = false
                };
                cbAutoBarecode.CheckedChanged += (s, e) =>
                {
                    if (cbAutoBarecode.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(txtBarCode.Text))
                        {
                            txtBarCode.Text = _functionUI.GenerateBarcode();
                        }
                        txtBarCode.ReadOnly = true;
                    }
                    else
                    {
                        txtBarCode.ReadOnly = false;
                    }
                };
                barcodePanel.Controls.Add(cbAutoBarecode);
            }
            _functionUI.AddComboBoxField(leftPanel, "Catégorie:", lblW, fldW, spc, out cbCategory);
            _functionUI.AddFormField(leftPanel, "Prix Achat:", lblW, fldW, 30, spc, out txtPurchasePrice, "N6", "0.000000");
            _functionUI.AddFormField(leftPanel, "Prix Vente:", lblW, fldW, 30, spc, out txtSalesPrice, "N6", "0.000000");


            _functionUI.AddFormField(leftPanel, "Stock Initial:", lblW, fldW, 30, spc, out txtStock);
            _functionUI.AddFormField(leftPanel, "Alerte Stock:", lblW, fldW, 30, spc, out txtAlertQty);
            _functionUI.AddComboBoxField(leftPanel, "TVA (%):", lblW, fldW, spc, out cbTaxe);
            cbTaxe.Items.AddRange(new object[] { "0%", "9%", "19%" });
            cbTaxe.SelectedIndex = 0;



            // AddSeparator(rightPanel, "Logistique & Classification");
            _functionUI.AddComboBoxField(rightPanel, "Unité:", lblW, fldW, spc, out cbUnit);
            _functionUI.AddFormField(rightPanel, "Colisage:", lblW, fldW, 30, spc, out txtColisage);
            _functionUI.AddComboBoxField(rightPanel, "Nature:", lblW, fldW, spc, out cbNature);
            _functionUI.AddComboBoxField(rightPanel, "Marque:", lblW, fldW, spc, out cbBrand);
            _functionUI.AddComboBoxField(rightPanel, "Favori:", lblW, fldW, spc, out cbIsFavorite);
            cbIsFavorite.Items.AddRange(new object[] { "Non", "Oui" }); cbIsFavorite.SelectedIndex = 0;


            // DataGridView for price listes
            Label lblPriceList = new Label
            {
                Text = "Tarification:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 5)
            };
            rightPanel.Controls.Add(lblPriceList);

            dgvPriceLists = new DataGridView
            {
                Size = new Size(500, 150),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = true,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 35
            };

            dgvPriceLists.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvPriceLists.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPriceLists.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvPriceLists.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(44, 62, 80);
            dgvPriceLists.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvPriceLists.DefaultCellStyle.Font = new Font("Segoe UI", 9);

            dgvPriceLists.Columns.Add("Name", "Tarification");
            dgvPriceLists.Columns.Add("Price", "Prix");
            dgvPriceLists.Columns[1].DefaultCellStyle.Format = "N6";

            dgvPriceLists.Columns[0].Width = 300;
            dgvPriceLists.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            rightPanel.Controls.Add(dgvPriceLists);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 10, 40, 0),
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

        }

        public  virtual async void BtnSave_Click(object? sender, EventArgs e)
        {
            try {
                bool Favorite;
                if (cbIsFavorite.SelectedIndex == 1)
                {
                    Favorite = true;
                }
                else
                {
                    Favorite = false;
                }
                var produitDto = new ProduitDto
            {
                RefProduct = txtRef.Text,
                Designation = txtDesignation.Text,              
                Taxe = cbTaxe.Text,
                BarCode = txtBarCode.Text,
                CategoryId = (cbCategory.SelectedItem as Category)?.Id,
                UnitId = (cbUnit.SelectedItem as Unit)?.Id,
                MarqueId = (cbBrand.SelectedItem as Marque)?.Id,
                NatureId = (cbNature.SelectedItem as Nature)?.Id,
                Colisage = (int?)_functionUI.ParseDecimal(txtColisage.Text),
                PurchasePrice = _functionUI.ParseDecimal(txtPurchasePrice.Text),
                SalesPrice = _functionUI.ParseDecimal(txtSalesPrice.Text),
                StockQuantity = _functionUI.ParseDecimal(txtStock.Text),
                QtyAlert = _functionUI.ParseDecimal(txtAlertQty.Text),
                IsFavorite = Favorite
                };
            await _service.AddAsync(produitDto);
            await SavePriceList(dgvPriceLists, produitDto.Id);

                MessageBox.Show("Produit ajouté avec succès");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
        }

        public virtual async Task SavePriceList(DataGridView dataGridView, int  IdProduct)
        {
            // Save Price Lists
           
            if (dataGridView.Rows.Count > 0)
            {
                bool hasPriceLists = false;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;
                    string name = row.Cells[0].Value?.ToString();
                    string priceVal = row.Cells[1].Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var pl = new PriceLists
                        {
                            ProductId = IdProduct,
                            Name = name,
                            Price = Convert.ToDecimal(priceVal)
                        };
                        await _appContext.PriceLists.AddAsync(pl);
                        hasPriceLists = true;
                    }
                }
                if (hasPriceLists)
                {
                    await _appContext.SaveChangesAsync();
                }
            }
        }
    }
}
