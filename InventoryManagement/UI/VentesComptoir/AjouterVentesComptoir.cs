using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.VentesComptoir
{
    public partial class AjouterVentesComptoir : Form
    {
        // UI Controls
        private Label lblTotalDisplay;
        private TextBox txtSearch;
        private DataGridView dgvCart;
        private Label lblChange;
        private Button btnValidate;
        private Button btnCancel;
        private Panel pnlHeader;
        private Panel pnlFooter;
        // New Fields
        private TextBox txtClient;
        private TextBox txtTicketNumber;
        private DateTimePicker dtpDate;
        // Footer numeric fields
        private NumericUpDown numTotalHT;
        private NumericUpDown numRemise;
        private NumericUpDown numTotalHTRemise;
        private NumericUpDown numTotalTVA;
        private NumericUpDown numTotalTTC;
        private NumericUpDown numMontantPaye;
        private NumericUpDown numResteAPayer;
        private ComboBox cbxCaisse;
        // Favoris
        private Panel pnlFavoritesScroll;
        private FlowLayoutPanel pnlFavoritesFlow;

        // Data / State
        private decimal _totalTTC = 0;
        private decimal _change = 0;
        private TabControl tabRightPanel;
        private FlowLayoutPanel pnlPendingFlow;
        private Panel pnlPendingScroll;
        private Button btnHold;
        public readonly AppDbContext _appContext;
        private readonly IService<SalesInvoicesDto> _service;
        private readonly ClientService _clientService;
        private readonly ProduitRepository _produitRepository;
        private readonly IService<SalesInvoiceLineDto> _lineService;
        private List<Product> _cachedProducts = new List<Product>();
        private readonly CratesRepository _cratesRepository;


        public AjouterVentesComptoir( AppDbContext appdbContext , IService<SalesInvoicesDto> service
            
           ,ClientService clientService,ProduitRepository produitRepository,IService<SalesInvoiceLineDto> service1
            , CratesRepository cratesRepository
            )
        {
            InitializeComponent();
            _appContext = appdbContext;
            _service = service;
            _clientService = clientService;
            _produitRepository  = produitRepository;
            _lineService = service1;
            _cratesRepository = cratesRepository;
            SetupCustomUI();

            // Set full screen/maximized for POS feel
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true; // For shortcuts
            this.KeyDown += AddCounterSalesForm_KeyDown;
            this.Load += async (s, e) => 
            { 
                await LoadCachedProductsAsync();
                LoadFavorites(); 
                LoadSearchAutoComplete(); 
                LoadPendingCarts(); 
                LoadClientAutoComplete(); 
            };
            this.txtSearch.Select();


        }
        private void SetupCustomUI()
        {
            this.Text = "Comptoir Caisse - POS";
            this.BackColor = Color.FromArgb(240, 243, 245); // Light Gray Background

            // ── Outer layout: left = main content (75%), right = favorites panel (25%) ──
            TableLayoutPanel outerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            outerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(outerLayout);

            // ── Main layout (left column) ──
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // Header/Total
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));  // Info Panel (Client, Ticket, Date)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));  // Search
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Grid
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 240F)); // Pied section
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));  // Buttons
            outerLayout.Controls.Add(mainLayout, 0, 0);

            // 1. Header Section (Total Display)
            pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(44, 62, 80), // Dark Blue
                Padding = new Padding(20),
                Height = 50
            };

            lblTotalDisplay = new Label
            {
                Text = "0.00 DA",
                Dock = DockStyle.Right,
                Font = new Font("Segoe UI", 48, FontStyle.Bold),
                ForeColor = Color.White, // Neon Green/White
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            };

            Label lblStoreName = new Label
            {
                Text = "CAISSE ENREGISTREUSE",
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(189, 195, 199),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTotalDisplay);
            pnlHeader.Controls.Add(lblStoreName);
            mainLayout.Controls.Add(pnlHeader, 0, 0);

            // 1b. Info Section (Client, N° Ticket, Date)
            Panel pnlInfo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5),
                Height = 85
            };

            TableLayoutPanel infoLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Client
            infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Ticket
            infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Date
            infoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlInfo.Controls.Add(infoLayout);

            // Client control setup
            TableLayoutPanel cellClient = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(5)
            };
            cellClient.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            cellClient.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblClient = new Label
            {
                Text = "CLIENT",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            txtClient = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12),
            //    Text = "Client Comptoir"
            };
            txtClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtClient.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtClient.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            cellClient.Controls.Add(lblClient, 0, 0);
            cellClient.Controls.Add(txtClient, 0, 1);
            infoLayout.Controls.Add(cellClient, 0, 0);

            // Ticket control setup
            TableLayoutPanel cellTicket = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(5)
            };
            cellTicket.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            cellTicket.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblTicket = new Label
            {
                Text = "N° TICKET",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            txtTicketNumber = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12),
                Text = "TCK-" + DateTime.Now.ToString("HHmmss")
            };
            cellTicket.Controls.Add(lblTicket, 0, 0);
            cellTicket.Controls.Add(txtTicketNumber, 0, 1);
            infoLayout.Controls.Add(cellTicket, 1, 0);

            // Date control setup
            TableLayoutPanel cellDate = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(5)
            };
            cellDate.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            cellDate.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblDate = new Label
            {
                Text = "DATE",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft
            };
            dtpDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            cellDate.Controls.Add(lblDate, 0, 0);
            cellDate.Controls.Add(dtpDate, 0, 1);
            infoLayout.Controls.Add(cellDate, 2, 0);

            mainLayout.Controls.Add(pnlInfo, 0, 1);

            // 2. Search Bar Section
            Panel searchPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 20),
                ForeColor = Color.Black,
                //PlText = "🔍 Scanner le code-barres ou rechercher un produit (F1)..."
            };
            // Autocomplete settings (will be populated on Load)
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSearch.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);
            mainLayout.Controls.Add(searchPanel, 0, 2);

            // 3. Grid Section (Smart Cart)
            dgvCart = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 40 },
                ColumnHeadersHeight = 40,
                Font = new Font("Segoe UI", 12)
            };

            // Grid Styling
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 240, 241);
            dgvCart.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Columns
            //dgvCart.Columns.Add("Ref", "Réf");
            //dgvCart.Columns.Add("Designation", "Désignation");
            //dgvCart.Columns.Add("CodeBarre", "Code Barre");
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "IdProduct", ReadOnly = true });
            dgvCart.Columns["IdProduct"].Visible = false;
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Réf", Name = "Ref", ReadOnly = true, FillWeight = 150 });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Désignation", Name = "Designation", ReadOnly = true , FillWeight = 300 });
            dgvCart.Columns.Add(new DataGridViewComboBoxColumn { HeaderText = "Tarification", Name = "Tarification", ReadOnly = false });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Code Barre", Name = "CodeBarre", ReadOnly = true });

            // Qty Column (Editable)
            var qtyCol = new DataGridViewTextBoxColumn { Name = "Qte", HeaderText = "Qté" };
            dgvCart.Columns.Add(qtyCol);

            dgvCart.Columns.Add("Prix", "Prix");
            // dgvCart.Columns.Add("TVA", "TVA");
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Qte Pack", Name = "QtePack" });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TVA", Name = "TVA", ReadOnly = true });

            //dgvCart.Columns.Add("TotalHT", "Total HT");
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total HT", Name = "TotalHT", ReadOnly = true });

            // Delete Button Column
            DataGridViewButtonColumn btnDelCol = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "❌",
                UseColumnTextForButtonValue = true,
                Name = "Delete",
                Width = 50
            };
            dgvCart.Columns.Add(btnDelCol);

            dgvCart.CellValueChanged += DgvCart_CellValueChanged;
            dgvCart.CurrentCellDirtyStateChanged += DgvCart_CurrentCellDirtyStateChanged;
            dgvCart.CellContentClick += DgvCart_CellContentClick;

            // Clic droit sur le panier → toggle favori
            var cartCtxMenu = new ContextMenuStrip();
            var menuAddFav = new ToolStripMenuItem("⭐  Ajouter aux favoris");
            menuAddFav.Click += DgvCartContextMenu_AddFavorite;
            var menuRemoveFav = new ToolStripMenuItem("☆  Retirer des favoris");
            menuRemoveFav.Click += DgvCartContextMenu_RemoveFavorite;
            cartCtxMenu.Items.Add(menuAddFav);
            cartCtxMenu.Items.Add(new ToolStripSeparator());
            cartCtxMenu.Items.Add(menuRemoveFav);
            dgvCart.ContextMenuStrip = cartCtxMenu;

            mainLayout.Controls.Add(dgvCart, 0, 3);

            // 4. Pied Section (totaux + caisse)
            Panel pnlPiedSection = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            Label lblPied = new Label
            {
                Text = "Pied",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlPiedSection.Controls.Add(lblPied);

            Panel piedPanel = new Panel
            {
                Location = new Point(10, 40),
                Size = new Size(1120, 180),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlPiedSection.Controls.Add(piedPanel);

            int piedY = 10;
            int labelWidth = 150;
            int fieldWidth = 200;

            AddPiedField(piedPanel, "Total HT:", ref piedY, labelWidth, fieldWidth, out numTotalHT);
            numTotalHT.ReadOnly = true;
            numTotalHT.BackColor = Color.FromArgb(236, 240, 241);

            AddPiedField(piedPanel, "Remise:", ref piedY, labelWidth, fieldWidth, out numRemise);
            numRemise.ValueChanged += CalculateTotals;

            AddPiedField(piedPanel, "Total HT Remisé:", ref piedY, labelWidth, fieldWidth, out numTotalHTRemise);
            numTotalHTRemise.ReadOnly = true;
            numTotalHTRemise.BackColor = Color.FromArgb(236, 240, 241);

            AddPiedField(piedPanel, "Total TVA:", ref piedY, labelWidth, fieldWidth, out numTotalTVA);
            numTotalTVA.ReadOnly = true;
            numTotalTVA.BackColor = Color.FromArgb(236, 240, 241);

            AddPiedField(piedPanel, "Total TTC:", ref piedY, labelWidth, fieldWidth, out numTotalTTC);
            numTotalTTC.ReadOnly = true;
            numTotalTTC.BackColor = Color.FromArgb(236, 240, 241);
            numTotalTTC.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            piedY = 10;
            int rightColumnX = 600;

            AddPiedFieldRight(piedPanel, "Montant Payé:", ref piedY, rightColumnX, labelWidth, fieldWidth, out numMontantPaye);
            numMontantPaye.ValueChanged += CalculateTotals;

            AddPiedFieldRight(piedPanel, "Reste à Payer:", ref piedY, rightColumnX, labelWidth, fieldWidth, out numResteAPayer);
            numResteAPayer.ReadOnly = true;
            numResteAPayer.BackColor = Color.FromArgb(255, 235, 235);
            numResteAPayer.ForeColor = Color.FromArgb(192, 57, 43);
            numResteAPayer.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            AddPiedFieldRight(piedPanel, "Caise:", ref piedY, rightColumnX, labelWidth, fieldWidth, out cbxCaisse);

            var crates = _appContext.Crates.ToList();
            cbxCaisse.DataSource = crates;
            cbxCaisse.DisplayMember = "Name";
            cbxCaisse.ValueMember = "Id";

            mainLayout.Controls.Add(pnlPiedSection, 0, 4);

            // 5. Action Buttons
            pnlFooter = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            TableLayoutPanel buttonLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnHold = new Button
            {
                Text = "METTRE EN ATTENTE",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnHold.FlatAppearance.BorderSize = 0;
            btnHold.Click += (s, e) => SavePendingCart();

            btnValidate = new Button
            {
                Text = "VALIDER (F12)",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnValidate.FlatAppearance.BorderSize = 0;
            btnValidate.Click += BtnValidate_Click;

            buttonLayout.Controls.Add(btnHold, 0, 0);
            buttonLayout.Controls.Add(btnValidate, 1, 0);
            pnlFooter.Controls.Add(buttonLayout);
            mainLayout.Controls.Add(pnlFooter, 0, 5);

            // ── Panneau droit (favoris + paniers en attente) ──
            outerLayout.Controls.Add(SetupRightPanel(), 1, 0);
        }

        // Logic
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string query = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(query))
                {
                    SearchAndAddProduct(query);
                    txtSearch.Clear();
                    txtSearch.Focus();
                }
                e.Handled = true;
                e.SuppressKeyPress = true; // Prevent ding sound
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();
            if (query.Length >= 4)
            {
                var product = _cachedProducts.FirstOrDefault(p => p.BarCode == query);
                if (product != null)
                {
                    txtSearch.TextChanged -= TxtSearch_TextChanged;
                    txtSearch.Clear();
                    AddToGrid(product, bypassDialog: false);
                    txtSearch.Focus();
                    txtSearch.TextChanged += TxtSearch_TextChanged;
                }
            }
        }

        private void SearchAndAddProduct(string query)
        {
            // Try Exact Barcode Match first in memory
            var product = _cachedProducts.FirstOrDefault(p => p.BarCode == query);
            if (product != null)
            {
                AddToGrid(product, bypassDialog: false);
                return;
            }

            // If not found, try Exact Ref in memory
            product = _cachedProducts.FirstOrDefault(p => p.RefProduct == query);
            if (product != null)
            {
                AddToGrid(product, bypassDialog: false);
                return;
            }

            // If not found, try partial Designation logic in memory
            product = _cachedProducts.FirstOrDefault(p => p.Designation != null && p.Designation.Contains(query, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                AddToGrid(product, bypassDialog: false);
                return;
            }

            // If still not found, query database
            using (var db = new AppDbContext())
            {
                product = db.Products.AsNoTracking().Include(p => p.PriceLists).FirstOrDefault(p => p.BarCode == query);
                if (product != null)
                {
                    _cachedProducts.Add(product);
                    AddToGrid(product, bypassDialog: false);
                    return;
                }

                product = db.Products.AsNoTracking().Include(p => p.PriceLists).FirstOrDefault(p => p.RefProduct == query);
                if (product != null)
                {
                    _cachedProducts.Add(product);
                    AddToGrid(product, bypassDialog: false);
                    return;
                }

                product = db.Products.AsNoTracking().Include(p => p.PriceLists).FirstOrDefault(p => p.Designation.Contains(query));
                if (product != null)
                {
                    _cachedProducts.Add(product);
                    AddToGrid(product, bypassDialog: false);
                    return;
                }
            }

            System.Media.SystemSounds.Beep.Play();
        }

        private void AddToGrid(Product product, bool bypassDialog = false)
        {
            decimal chosenQty = 1;
            decimal chosenPrice = product.SalesPrice ?? 0;
            decimal chosenQtePack = 0;
            object chosenTarifVal = product.SalesPrice ?? 0m;

            if (!bypassDialog)
            {
                try
                {
                    using (var scope = Program.ServiceProvider.CreateScope())
                    {
                        var qtePrixForm = scope.ServiceProvider.GetRequiredService<Qte__Prix>();
                        qtePrixForm.LoadProductData(product);
                        if (qtePrixForm.ShowDialog() != DialogResult.OK)
                        {
                            return; // User cancelled
                        }

                        decimal.TryParse(qtePrixForm.Qte.Text, out chosenQty);
                        decimal.TryParse(qtePrixForm.Prix.Text, out chosenPrice);
                        decimal.TryParse(qtePrixForm.Qte_pack.Text, out chosenQtePack);
                        chosenTarifVal = qtePrixForm.Tarification.SelectedValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'ouverture de la saisie de quantité/prix: " + ex.Message);
                    return;
                }
            }

            // Check if exists
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                if (row.Cells["Ref"].Value.ToString() == product.RefProduct)
                {
                    // Increment Qty
                    decimal currentQty = Convert.ToDecimal(row.Cells["Qte"].Value);
                    row.Cells["Qte"].Value = currentQty + chosenQty;

                    decimal currentQtePack = Convert.ToDecimal(row.Cells["QtePack"].Value ?? 0);
                    row.Cells["QtePack"].Value = currentQtePack + chosenQtePack;

                    row.Cells["Prix"].Value = chosenPrice;

                    if (row.Cells["Tarification"] is DataGridViewComboBoxCell tarifCell)
                    {
                        tarifCell.Value = chosenTarifVal;
                    }

                    UpdateRowCalculations(row);
                    UpdateTotal();
                    return;
                }
            }

            // Add New
            int index = dgvCart.Rows.Add(
                product.Id,
                product.RefProduct,
                product.Designation,
                null,
                product.BarCode,
                chosenQty,
                chosenPrice,
                chosenQtePack,
                product.Taxe ?? "0",
                0 // TotalHT placeholder
            );

            // Populate Tarification
            try
            {
                var options = new List<PriceOption>();
                options.Add(new PriceOption { Display = $"Standard ({product.SalesPrice:N6})", Value = product.SalesPrice });
                if (product.PriceLists != null)
                {
                    foreach (var pl in product.PriceLists)
                    {
                        options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N6})", Value = pl.Price });
                    }
                }

                var tarifCell = (DataGridViewComboBoxCell)dgvCart.Rows[index].Cells["Tarification"];
                tarifCell.DataSource = options;
                tarifCell.DisplayMember = "Display";
                tarifCell.ValueMember = "Value";
                tarifCell.Value = chosenTarifVal;
            }
            catch { }

            UpdateRowCalculations(dgvCart.Rows[index]);
            UpdateTotal();

            // Scroll to bottom
            dgvCart.FirstDisplayedScrollingRowIndex = index;
            dgvCart.CurrentCell = dgvCart.Rows[index].Cells["Qte"]; // Focus on Qte for quick edit
            dgvCart.Rows[index].Selected = true;
        }

        private void DgvCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string colName = dgvCart.Columns[e.ColumnIndex].Name;
                if (colName == "Qte" || colName == "Prix" || colName == "Tarification")
                {
                    if (colName == "Tarification")
                    {
                        var row = dgvCart.Rows[e.RowIndex];
                        if (row.Cells["Tarification"].Value != null)
                        {
                            if (decimal.TryParse(row.Cells["Tarification"].Value.ToString(), out decimal p))
                            {
                                dgvCart.CellValueChanged -= DgvCart_CellValueChanged;
                                row.Cells["Prix"].Value = p;
                                dgvCart.CellValueChanged += DgvCart_CellValueChanged;
                            }
                        }
                    }
                    UpdateRowCalculations(dgvCart.Rows[e.RowIndex]);
                    UpdateTotal();
                }
                else if (colName == "QtePack")
                {
                    CalculateQteFromPack(e.RowIndex);
                    UpdateTotal();
                }
            }
        }

        private void DgvCart_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCart.CurrentCell is DataGridViewComboBoxCell && dgvCart.IsCurrentCellDirty)
            {
                dgvCart.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void CalculateQteFromPack(int rowIndex)
        {
            try
            {
                var row = dgvCart.Rows[rowIndex];
                string refProduct = row.Cells["Ref"].Value?.ToString();
                object qtePackObj = row.Cells["QtePack"].Value;

                if (string.IsNullOrEmpty(refProduct) || qtePackObj == null) return;

                if (decimal.TryParse(qtePackObj.ToString(), out decimal qtePack))
                {
                    using (var db = new AppDbContext())
                    {
                        var product = db.Products.AsNoTracking().FirstOrDefault(p => p.RefProduct == refProduct);
                        if (product != null && product.Colisage.HasValue && product.Colisage.Value > 0)
                        {
                            dgvCart.CellValueChanged -= DgvCart_CellValueChanged;
                            row.Cells["Qte"].Value = (qtePack * product.Colisage.Value).ToString("G29");
                            dgvCart.CellValueChanged += DgvCart_CellValueChanged;
                            UpdateRowCalculations(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du calcul de la quantité colisage: " + ex.Message);
            }
        }

        private void DgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCart.Columns[e.ColumnIndex].Name == "Delete")
            {
                dgvCart.Rows.RemoveAt(e.RowIndex);
                UpdateTotal();
            }
        }

        private void UpdateRowCalculations(DataGridViewRow row)
        {
            if (row == null) return;

            decimal qty = 0;
            decimal.TryParse(row.Cells["Qte"].Value?.ToString(), out qty);

            decimal price = 0;
            decimal.TryParse(row.Cells["Prix"].Value?.ToString(), out price);

            dgvCart.CellValueChanged -= DgvCart_CellValueChanged;
            row.Cells["TotalHT"].Value = (qty * price).ToString("N6");
            dgvCart.CellValueChanged += DgvCart_CellValueChanged;
        }

        private void UpdateTotal()
        {
            decimal totalHT = 0;
            decimal totalTTC = 0;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                decimal rowHT = 0;
                decimal.TryParse(row.Cells["TotalHT"].Value?.ToString(), out rowHT);
                totalHT += rowHT;

                // Simplified tax calc
                string tvaStr = row.Cells["TVA"].Value?.ToString() ?? "0";
                decimal tvaPercent = 0;
                if (tvaStr.Contains("%")) decimal.TryParse(tvaStr.Replace("%", ""), out tvaPercent);
                else decimal.TryParse(tvaStr, out tvaPercent);

                decimal rowTax = rowHT * (tvaPercent / 100);
                totalTTC += rowHT + rowTax;
            }

            _totalTTC = totalTTC;
            lblTotalDisplay.Text = _totalTTC.ToString("N6") + " DA";
            CalculateChange();
        }

        private void CalculateChange()
        {
            // Recompute totals from the cart and update footer numeric fields
            decimal totalHT = 0m;
            decimal totalTVA = 0m;

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                if (row.IsNewRow) continue;
                decimal rowHT = 0m;
                decimal.TryParse(row.Cells["TotalHT"].Value?.ToString(), out rowHT);
                totalHT += rowHT;

                string tvaStr = row.Cells["TVA"].Value?.ToString() ?? "0";
                decimal tvaPercent = 0m;
                if (tvaStr.Contains("%")) decimal.TryParse(tvaStr.Replace("%", ""), out tvaPercent);
                else decimal.TryParse(tvaStr, out tvaPercent);

                decimal rowTax = rowHT * (tvaPercent / 100m);
                totalTVA += rowTax;
            }

            decimal remise = 0m;
            try { remise = numRemise?.Value ?? 0m; } catch { remise = 0m; }

            decimal totalHTRemise = totalHT - remise;
            if (totalHTRemise < 0) totalHTRemise = 0m;

            decimal totalTTC = totalHTRemise + totalTVA;

            // Helper to safely assign NumericUpDown values within range
            void SetNudValue(NumericUpDown nud, decimal val)
            {
                if (nud == null) return;
                if (val < nud.Minimum) val = nud.Minimum;
                if (val > nud.Maximum) val = nud.Maximum;
                try { nud.Value = decimal.Round(val, nud.DecimalPlaces); } catch { nud.Value = nud.Minimum; }
            }

            numRemise.ValueChanged -= CalculateTotals;
            numMontantPaye.ValueChanged -= CalculateTotals;

            SetNudValue(numTotalHT, totalHT);
            SetNudValue(numTotalTVA, totalTVA);
            SetNudValue(numTotalHTRemise, totalHTRemise);
            SetNudValue(numTotalTTC, totalTTC);
            SetNudValue(numMontantPaye, totalTTC);

            numRemise.ValueChanged += CalculateTotals;
            numMontantPaye.ValueChanged += CalculateTotals;

            _totalTTC = totalTTC;
            lblTotalDisplay.Text = _totalTTC.ToString("N6") + " DA";

            RecalculateBalance();

         //   decimal paid = numPayment.Value;
          //  _change = paid - _totalTTC;
            //lblChange.Text = "Rendu: " + (_change > 0 ? _change.ToString("N2") : "0.00") + " DA";
            //lblChange.ForeColor = _change >= 0 ? Color.Green : Color.Red;
        }

        private async void BtnValidate_Click(object sender, EventArgs e)
        {
            if (!CheckdgvArticlesRows())
                return;
            var context = _appContext;
            var clientName = txtClient.Text.Trim().ToLower();
            var selectedCustomer = context.Customers
                .FirstOrDefault(c => c.Name.ToLower() == clientName);

            if (selectedCustomer == null && (decimal)numResteAPayer.Value > 0)
            {
                MessageBox.Show("Veuillez sélectionner un client pour les paiements en attente!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var salesInvoicesDto = new SalesInvoicesDto
                {
                    NumberInvoice = txtTicketNumber.Text,
                    DateInvoice = dtpDate.Value,
                    IdCustomer = selectedCustomer?.Id,
                    TotalWithoutTax = (decimal)numTotalHT.Value,
                    Remise = (decimal)numRemise.Value,
                    TotalWithoutTaxRemise = (decimal)numTotalHTRemise.Value,
                    TotalTax = (decimal)numTotalTVA.Value,
                    TotalInvoice = (decimal)numTotalTTC.Value,
                    PaymentInvoice = (decimal)numMontantPaye.Value,
                    BalanceInvoice = (decimal)numResteAPayer.Value,
                    IdCrates = (cbxCaisse.SelectedItem as Crates)?.Id,
                };
                await _service.AddAsync(salesInvoicesDto);
                await SaveLinesProducts(dgvCart, salesInvoicesDto.Id, context);
                await _cratesRepository.AddMoney((cbxCaisse.SelectedItem as Crates).Id, (decimal)numMontantPaye.Value);

                if (selectedCustomer != null)
                {
                    await _clientService.UpdateBalanceAsync(selectedCustomer.Id, (decimal)numResteAPayer.Value);
                }
                if (selectedCustomer != null)
                {
                    await _clientService.UpdateTurnoverAsync(selectedCustomer.Id, (decimal)numTotalTTC.Value);
                }
                MessageBox.Show("Vente ajoutée avec succès");
                // this.DialogResult = DialogResult.OK;
                Clear();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Clear()
        {
            // Cart
            dgvCart.Rows.Clear();

            // Info section
            txtClient.Clear();
            txtTicketNumber.Text = "TCK-" + DateTime.Now.ToString("HHmmss");
            dtpDate.Value = DateTime.Now;

            // Search bar
            txtSearch.Clear();

            // Footer numeric fields
            numRemise.ValueChanged -= CalculateTotals;
            numMontantPaye.ValueChanged -= CalculateTotals;

            numTotalHT.Value = 0;
            numRemise.Value = 0;
            numTotalHTRemise.Value = 0;
            numTotalTVA.Value = 0;
            numTotalTTC.Value = 0;
            numMontantPaye.Value = 0;
            numResteAPayer.Value = 0;

            numRemise.ValueChanged += CalculateTotals;
            numMontantPaye.ValueChanged += CalculateTotals;

            // Header total display
            _totalTTC = 0;
            _change = 0;
            lblTotalDisplay.Text = "0.00 DA";

            txtSearch.Focus();
        }

        public async Task SaveLinesProducts(DataGridView dataGridView, int idSalesInvoice, AppDbContext context)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                string IdProduct = row.Cells["IdProduct"].Value?.ToString();
                decimal qteVendue = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0);
                var product = context.Products.FirstOrDefault(p => p.Id.ToString() == IdProduct);
                if (product == null)
                    continue;
                _produitRepository.ModifierQty(product.Id, (int)qteVendue, false);
                var salesInvoiceLineDto = new SalesInvoiceLineDto
                {
                    IdProduct = Convert.ToInt32(row.Cells["IdProduct"].Value),
                    RefProduct = row.Cells["Ref"].Value?.ToString(),
                    Designation = row.Cells["Designation"].Value?.ToString(),
                    Quantity = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0),
                    Price = Convert.ToDecimal(row.Cells["Prix"].Value ?? 0),
                    Taxe = row.Cells["TVA"].Value?.ToString() ?? "0", // Default tax
                    TotalWithoutTax = Convert.ToDecimal(row.Cells["TotalHT"].Value ?? 0)
                };
                salesInvoiceLineDto.IdSalesInvoice = idSalesInvoice;
                await _lineService.AddAsync(salesInvoiceLineDto);
            }
        }
        private void AddCounterSalesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                txtSearch.Focus();
            }
            else if (e.KeyCode == Keys.F12)
            {
                BtnValidate_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void CalculateTotals(object? sender, EventArgs? e)
        {
            CalculateChange();
        }

        private void RecalculateBalance()
        {
            decimal paid = numMontantPaye?.Value ?? 0m;
            decimal balance = _totalTTC - paid;
            if (balance < 0) balance = 0m;

            if (numResteAPayer != null)
            {
                if (balance < numResteAPayer.Minimum) balance = numResteAPayer.Minimum;
                if (balance > numResteAPayer.Maximum) balance = numResteAPayer.Maximum;
                try { numResteAPayer.Value = decimal.Round(balance, numResteAPayer.DecimalPlaces); }
                catch { numResteAPayer.Value = numResteAPayer.Minimum; }
            }
        }

        // ═══════════════════════════════════════════════════
        //  FAVORIS — Panneau, chargement, interaction
        // ═══════════════════════════════════════════════════

        private Control SetupRightPanel()
        {
            tabRightPanel = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ItemSize = new Size(160, 36),
                SizeMode = TabSizeMode.Fixed
            };

            var favoritesTab = new TabPage("Favoris");
            var favoritesWrapper = CreateFavoritesPanel();
            favoritesTab.Controls.Add(favoritesWrapper);

            var pendingTab = new TabPage("Paniers en attente");
            var pendingWrapper = CreatePendingPanel();
            pendingTab.Controls.Add(pendingWrapper);

            tabRightPanel.TabPages.Add(favoritesTab);
            tabRightPanel.TabPages.Add(pendingTab);

            return tabRightPanel;
        }

        private Panel CreateFavoritesPanel()
        {
            Panel favWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 35, 45)
            };

            // ── En-tête ──
            Panel favHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(12, 8, 12, 8)
            };
            Label lblFavTitle = new Label
            {
                Text = "⭐  PRODUITS FAVORIS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(241, 196, 15),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            favHeader.Controls.Add(lblFavTitle);

            // ── Bouton Actualiser (bas) ──
            Button btnRefresh = new Button
            {
                Text = "🔄  Actualiser",
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.FromArgb(189, 195, 199),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 90, 110);
            btnRefresh.Click += (s, e) => LoadFavorites();

            // ── Conteneur scrollable ──
            pnlFavoritesScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(25, 35, 45),
                Padding = new Padding(0)
            };

            pnlFavoritesFlow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(25, 35, 45),
                Padding = new Padding(6)
            };

            pnlFavoritesScroll.Controls.Add(pnlFavoritesFlow);
            favWrapper.Controls.Add(pnlFavoritesScroll);
            favWrapper.Controls.Add(btnRefresh);
            favWrapper.Controls.Add(favHeader);

            return favWrapper;
        }

        private Panel CreatePendingPanel()
        {
            Panel pendingWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 35, 45)
            };

            Panel pendingHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(12, 8, 12, 8)
            };
            Label lblPendingTitle = new Label
            {
                Text = "🧺  PANIERS EN ATTENTE",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(241, 196, 15),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pendingHeader.Controls.Add(lblPendingTitle);

            Button btnPendingRefresh = new Button
            {
                Text = "🔄  Actualiser",
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.FromArgb(189, 195, 199),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnPendingRefresh.FlatAppearance.BorderSize = 0;
            btnPendingRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 90, 110);
            btnPendingRefresh.Click += (s, e) => LoadPendingCarts();

            pnlPendingScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(25, 35, 45),
                Padding = new Padding(0)
            };

            pnlPendingFlow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(25, 35, 45),
                Padding = new Padding(6)
            };

            pnlPendingScroll.Controls.Add(pnlPendingFlow);
            pendingWrapper.Controls.Add(pnlPendingScroll);
            pendingWrapper.Controls.Add(btnPendingRefresh);
            pendingWrapper.Controls.Add(pendingHeader);

            return pendingWrapper;
        }

        private void LoadFavorites()
        {
            if (pnlFavoritesFlow == null || pnlFavoritesScroll == null) return;

            pnlFavoritesFlow.Controls.Clear();
            // Largeur des cartes = largeur du panneau - scrollbar - padding
            pnlFavoritesFlow.Width = Math.Max(pnlFavoritesScroll.ClientSize.Width, 10);

            Color[] palette = new[]
            {
                Color.FromArgb(52,  152, 219),  // Bleu
                Color.FromArgb(46,  204, 113),  // Vert
                Color.FromArgb(155,  89, 182),  // Violet
                Color.FromArgb(230, 126,  34),  // Orange
                Color.FromArgb(26,  188, 156),  // Turquoise
                Color.FromArgb(231,  76,  60),  // Rouge
                Color.FromArgb(241, 196,  15),  // Jaune
                Color.FromArgb( 52, 172, 180),  // Cyan
            };

            try
            {
                using (var db = new AppDbContext())
                {
                    var favorites = db.Products
                        .Where(p => p.IsFavorite)
                        .OrderBy(p => p.Designation)
                        .ToList();

                    if (favorites.Count == 0)
                    {
                        Label lblEmpty = new Label
                        {
                            Text = "Aucun produit favori.\n\nFait un clic droit sur\nun article du panier\npour l'ajouter aux\nfavoris.",
                            Font = new Font("Segoe UI", 10),
                            ForeColor = Color.FromArgb(100, 130, 150),
                            TextAlign = ContentAlignment.TopCenter,
                            AutoSize = false,
                            Width = Math.Max(pnlFavoritesScroll.ClientSize.Width - 20, 180),
                            Height = 150,
                            Padding = new Padding(10)
                        };
                        pnlFavoritesFlow.Controls.Add(lblEmpty);
                        return;
                    }

                    int ci = 0;
                    foreach (var product in favorites)
                    {
                        var card = CreateFavoriteCard(product, palette[ci % palette.Length]);
                        pnlFavoritesFlow.Controls.Add(card);
                        ci++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadFavorites error: " + ex.Message);
            }
        }

        private Panel CreateFavoriteCard(Product product, Color bgColor)
        {
            int cardW = Math.Max(pnlFavoritesScroll.ClientSize.Width - 18, 200);
            Color hoverColor = Color.FromArgb(
                Math.Min(255, bgColor.R + 30),
                Math.Min(255, bgColor.G + 30),
                Math.Min(255, bgColor.B + 30));

            Panel card = new Panel
            {
                Width  = cardW,
                Height = 82,
                BackColor = bgColor,
                Cursor    = Cursors.Hand,
                Margin    = new Padding(3, 4, 3, 0),
                Tag       = product
            };

            Label lblName = new Label
            {
                Text      = product.Designation,
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = false,
                Location  = new Point(10, 8),
                Size      = new Size(cardW - 50, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblPrice = new Label
            {
                Text      = $"{product.SalesPrice:N2} DA  /  {product.StockQuantity} ",
                Font      = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(225, 240, 240),
                AutoSize  = false,
                Location  = new Point(10, 42),
                Size      = new Size(cardW - 50, 26),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Bouton ★ pour retirer des favoris
            Button btnStar = new Button
            {
                Text      = "★",
                Location  = new Point(cardW - 42, 7),
                Size      = new Size(34, 34),
                BackColor = bgColor,
                ForeColor = Color.FromArgb(255, 235, 59),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 14),
                Cursor    = Cursors.Hand,
                TabStop   = false
            };
            btnStar.FlatAppearance.BorderSize = 0;
            btnStar.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Max(0, bgColor.R - 20), Math.Max(0, bgColor.G - 20), Math.Max(0, bgColor.B - 20));
            int favId = product.Id;
            btnStar.Click += (s, e) => ToggleFavoriteById(favId, false);

            // Clic sur la carte → ajouter au panier
            EventHandler addToCart = (s, e) =>
            {
                try
                {
                    var cached = _cachedProducts.FirstOrDefault(p => p.Id == product.Id);
                    if (cached != null)
                    {
                        AddToGrid(cached);
                    }
                    else
                    {
                        using (var db = new AppDbContext())
                        {
                            var fresh = db.Products.Find(product.Id);
                            if (fresh != null) AddToGrid(fresh);
                        }
                    }
                    txtSearch.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur: " + ex.Message);
                }
            };
            card.Click   += addToCart;
            lblName.Click  += addToCart;
            lblPrice.Click += addToCart;

            // Effet hover
            Action<bool> setHover = (over) => card.BackColor = over ? hoverColor : bgColor;
            card.MouseEnter    += (s, e) => setHover(true);
            card.MouseLeave    += (s, e) => setHover(false);
            lblName.MouseEnter  += (s, e) => setHover(true);
            lblName.MouseLeave  += (s, e) => setHover(false);
            lblPrice.MouseEnter += (s, e) => setHover(true);
            lblPrice.MouseLeave += (s, e) => setHover(false);

            card.Controls.Add(lblName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(btnStar);

            return card;
        }

        private void ToggleFavoriteById(int productId, bool isFavorite)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var p = db.Products.Find(productId);
                    if (p != null)
                    {
                        p.IsFavorite = isFavorite;
                        db.SaveChanges();
                    }
                }
                LoadFavorites();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur favoris: " + ex.Message);
            }
        }

        private void DgvCartContextMenu_AddFavorite(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null) return;
            var idObj = dgvCart.CurrentRow.Cells["IdProduct"].Value;
            if (idObj != null && int.TryParse(idObj.ToString(), out int prodId))
                ToggleFavoriteById(prodId, true);
        }

        private void DgvCartContextMenu_RemoveFavorite(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null) return;
            var idObj = dgvCart.CurrentRow.Cells["IdProduct"].Value;
            if (idObj != null && int.TryParse(idObj.ToString(), out int prodId))
                ToggleFavoriteById(prodId, false);
        }

        private void LoadSearchAutoComplete()
        {
            try
            {
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var it in _cachedProducts)
                {
                    if (!string.IsNullOrWhiteSpace(it.RefProduct)) set.Add(it.RefProduct);
                    if (!string.IsNullOrWhiteSpace(it.Designation)) set.Add(it.Designation);
                }

                var ac = new AutoCompleteStringCollection();
                ac.AddRange(set.ToArray());
                txtSearch.AutoCompleteCustomSource = ac;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadSearchAutoComplete error: " + ex.Message);
            }
        }

        private async Task LoadCachedProductsAsync()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    _cachedProducts = await db.Products
                        .AsNoTracking()
                        .Include(p => p.PriceLists)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadCachedProductsAsync error: " + ex.Message);
            }
        }

        private void LoadClientAutoComplete()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var names = db.Customers.AsNoTracking()
                        .Select(c => c.Name)
                        .ToList();

                    var ac = new AutoCompleteStringCollection();
                    ac.AddRange(names.ToArray());
                    txtClient.AutoCompleteCustomSource = ac;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadClientAutoComplete error: " + ex.Message);
            }
        }

        private void SavePendingCart()
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("Aucun article dans le panier à mettre en attente.", "Panier en attente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = GetCurrentCartItems();
            if (items.Count == 0)
            {
                MessageBox.Show("Impossible d'enregistrer un panier vide.", "Panier en attente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var carts = LoadPendingCartsFromFile();
            string cartNumber = GetNextCartNumber(carts);
            var cart = new PendingCartStorage
            {
                CartNumber = cartNumber,
                CreatedAt = DateTime.Now,
                TotalWithoutTax = items.Sum(i => i.TotalWithoutTax),
                TotalTTC = _totalTTC,
                Items = items
            };
            carts.Add(cart);
            SavePendingCartsToFile(carts);

            ClearCurrentCart();
            LoadPendingCarts();
            MessageBox.Show($"Panier enregistré en attente : {cartNumber}", "Panier en attente", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<PendingCartItem> GetCurrentCartItems()
        {
            var items = new List<PendingCartItem>();
            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Ref"].Value == null) continue;

                decimal.TryParse(row.Cells["Qte"].Value?.ToString(), out decimal quantity);
                decimal.TryParse(row.Cells["Prix"].Value?.ToString(), out decimal price);
                decimal.TryParse(row.Cells["QtePack"].Value?.ToString(), out decimal qtePack);
                decimal.TryParse(row.Cells["TotalHT"].Value?.ToString(), out decimal totalWithoutTax);
                decimal.TryParse(row.Cells["Tarification"].Value?.ToString(), out decimal tarifValue);

                items.Add(new PendingCartItem
                {
                    IdProduct = Convert.ToInt32(row.Cells["IdProduct"].Value ?? 0),
                    RefProduct = row.Cells["Ref"].Value.ToString(),
                    Designation = row.Cells["Designation"].Value?.ToString() ?? string.Empty,
                    CodeBarre = row.Cells["CodeBarre"].Value?.ToString() ?? string.Empty,
                    Quantity = quantity,
                    Price = price,
                    QtePack = qtePack,
                    Taxe = row.Cells["TVA"].Value?.ToString() ?? "0",
                    TarificationValue = tarifValue,
                    TotalWithoutTax = totalWithoutTax
                });
            }
            return items;
        }

        private void ClearCurrentCart()
        {
            dgvCart.Rows.Clear();
            _totalTTC = 0;
            lblTotalDisplay.Text = "0.00 DA";
            if (numRemise != null) numRemise.Value = 0;
            if (numTotalHT != null) numTotalHT.Value = 0;
            if (numTotalHTRemise != null) numTotalHTRemise.Value = 0;
            if (numTotalTVA != null) numTotalTVA.Value = 0;
            if (numTotalTTC != null) numTotalTTC.Value = 0;
            if (numMontantPaye != null) numMontantPaye.Value = 0;
            if (numResteAPayer != null) numResteAPayer.Value = 0;
        }

        private void AddPiedField(Panel parent, string labelText, ref int yPos, int labelW, int fieldW, out NumericUpDown numericUpDown)
        {
            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Size = new Size(labelW, 25),
                Location = new Point(20, yPos),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(label);

            numericUpDown = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(fieldW, 25),
                Location = new Point(20 + labelW, yPos),
                Maximum = 99999999999999,
                DecimalPlaces = 6,
                ThousandsSeparator = true
            };
            parent.Controls.Add(numericUpDown);

            yPos += 30;
        }

        private void AddPiedFieldRight(Panel parent, string labelText, ref int yPos, int xPos, int labelW, int fieldW, out ComboBox comboBox)
        {
            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Size = new Size(labelW, 25),
                Location = new Point(xPos, yPos),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(label);
            comboBox = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(fieldW, 25),
                Location = new Point(xPos + labelW, yPos),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            parent.Controls.Add(comboBox);
            yPos += 30;
        }

        private void AddPiedFieldRight(Panel parent, string labelText, ref int yPos, int xPos, int labelW, int fieldW, out NumericUpDown numericUpDown)
        {
            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                Size = new Size(labelW, 25),
                Location = new Point(xPos, yPos),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(label);

            numericUpDown = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(fieldW, 25),
                Location = new Point(xPos + labelW, yPos),
                Maximum = 99999999999999,
                DecimalPlaces = 6,
                ThousandsSeparator = true
            };
            parent.Controls.Add(numericUpDown);

            yPos += 30;
        }

        private void LoadPendingCarts()
        {
            if (pnlPendingFlow == null || pnlPendingScroll == null) return;

            pnlPendingFlow.Controls.Clear();
            pnlPendingFlow.Width = Math.Max(pnlPendingScroll.ClientSize.Width, 10);

            Color[] palette = new[]
            {
                Color.FromArgb(52, 152, 219),
                Color.FromArgb(46, 204, 113),
                Color.FromArgb(155, 89, 182),
                Color.FromArgb(230, 126, 34),
                Color.FromArgb(26, 188, 156),
                Color.FromArgb(231, 76, 60),
                Color.FromArgb(241, 196, 15),
                Color.FromArgb(52, 172, 180),
            };

            var carts = LoadPendingCartsFromFile();
            if (carts.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Aucun panier en attente.",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(100, 130, 150),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Width = Math.Max(pnlPendingScroll.ClientSize.Width - 20, 180),
                    Height = 150,
                    Padding = new Padding(10)
                };
                pnlPendingFlow.Controls.Add(lblEmpty);
                return;
            }

            int ci = 0;
            foreach (var cart in carts.OrderByDescending(c => c.CreatedAt))
            {
                var card = CreatePendingCartCard(cart, palette[ci % palette.Length]);
                pnlPendingFlow.Controls.Add(card);
                ci++;
            }
        }

        private Panel CreatePendingCartCard(PendingCartStorage cart, Color bgColor)
        {
            int cardW = Math.Max(pnlPendingScroll.ClientSize.Width - 18, 220);
            Color hoverColor = Color.FromArgb(
                Math.Min(255, bgColor.R + 20),
                Math.Min(255, bgColor.G + 20),
                Math.Min(255, bgColor.B + 20));

            Panel card = new Panel
            {
                Width = cardW,
                Height = 130,
                BackColor = bgColor,
                Cursor = Cursors.Hand,
                Margin = new Padding(3, 4, 3, 0),
                Tag = cart
            };

            Label lblNumber = new Label
            {
                Text = cart.CartNumber,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Location = new Point(10, 8),
                Size = new Size(cardW - 20, 26),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblInfo = new Label
            {
                Text = $"Créé le {cart.CreatedAt:dd/MM/yyyy HH:mm}" + Environment.NewLine + $"Total: {cart.TotalTTC:N2} DA",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(225, 240, 240),
                AutoSize = false,
                Location = new Point(10, 36),
                Size = new Size(cardW - 20, 40),
                TextAlign = ContentAlignment.TopLeft
            };

            Button btnRestore = new Button
            {
                Text = "Restaurer",
                Size = new Size(90, 30),
                Location = new Point(10, 84),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.Click += (s, e) => RestorePendingCart(cart);

            Button btnDelete = new Button
            {
                Text = "Supprimer",
                Size = new Size(90, 30),
                Location = new Point(cardW - 100, 84),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeletePendingCart(cart);

            Action<bool> setHover = (over) => card.BackColor = over ? hoverColor : bgColor;
            card.MouseEnter += (s, e) => setHover(true);
            card.MouseLeave += (s, e) => setHover(false);
            lblNumber.MouseEnter += (s, e) => setHover(true);
            lblNumber.MouseLeave += (s, e) => setHover(false);
            lblInfo.MouseEnter += (s, e) => setHover(true);
            lblInfo.MouseLeave += (s, e) => setHover(false);

            card.Click += (s, e) => RestorePendingCart(cart);
            lblNumber.Click += (s, e) => RestorePendingCart(cart);
            lblInfo.Click += (s, e) => RestorePendingCart(cart);

            card.Controls.Add(lblNumber);
            card.Controls.Add(lblInfo);
            card.Controls.Add(btnRestore);
            card.Controls.Add(btnDelete);

            return card;
        }

        private void RestorePendingCart(PendingCartStorage cart)
        {
            if (dgvCart.Rows.Count > 0)
            {
                var answer = MessageBox.Show("Le panier courant sera remplacé par le panier en attente. Continuer ?", "Restaurer panier", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer != DialogResult.Yes)
                    return;
            }

            ClearCurrentCart();
            foreach (var item in cart.Items)
            {
                AddPendingCartItem(item);
            }
            UpdateTotal();
            txtSearch.Focus();
        }

        private void DeletePendingCart(PendingCartStorage cart)
        {
            var answer = MessageBox.Show($"Supprimer le panier {cart.CartNumber} ?", "Supprimer panier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes) return;

            var carts = LoadPendingCartsFromFile();
            carts.RemoveAll(x => x.CartNumber == cart.CartNumber && x.CreatedAt == cart.CreatedAt);
            SavePendingCartsToFile(carts);
            LoadPendingCarts();
        }

        private void AddPendingCartItem(PendingCartItem item)
        {
            int index = dgvCart.Rows.Add(
                item.IdProduct,
                item.RefProduct,
                item.Designation,
                null,
                item.CodeBarre,
                item.Quantity,
                item.Price,
                item.QtePack,
                item.Taxe,
                item.TotalWithoutTax
            );

            try
            {
                using (var db = new AppDbContext())
                {
                    var priceLists = db.PriceLists.Where(pl => pl.ProductId == item.IdProduct).ToList();
                    var options = new List<PriceOption>();
                    options.Add(new PriceOption { Display = $"Standard ({item.Price:N2})", Value = item.Price });
                    foreach (var pl in priceLists)
                    {
                        options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N2})", Value = pl.Price });
                    }

                    var tarifCell = (DataGridViewComboBoxCell)dgvCart.Rows[index].Cells["Tarification"];
                    tarifCell.DataSource = options;
                    tarifCell.DisplayMember = "Display";
                    tarifCell.ValueMember = "Value";
                    tarifCell.Value = item.TarificationValue;
                }
            }
            catch
            {
            }

            UpdateRowCalculations(dgvCart.Rows[index]);
        }

        private string GetNextCartNumber(List<PendingCartStorage> carts)
        {
            int max = 0;
            foreach (var cart in carts)
            {
                if (string.IsNullOrWhiteSpace(cart.CartNumber)) continue;
                var parts = cart.CartNumber.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int parsed))
                {
                    if (parsed > max) max = parsed;
                }
            }
            return $"PANIER-{(max + 1):D4}";
        }

        private string GetPendingCartStoragePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pending_carts.json");
        }

        private List<PendingCartStorage> LoadPendingCartsFromFile()
        {
            try
            {
                var path = GetPendingCartStoragePath();
                if (!File.Exists(path)) return new List<PendingCartStorage>();
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<PendingCartStorage>>(json) ?? new List<PendingCartStorage>();
            }
            catch
            {
                return new List<PendingCartStorage>();
            }
        }

        private void SavePendingCartsToFile(List<PendingCartStorage> carts)
        {
            try
            {
                var path = GetPendingCartStoragePath();
                var json = JsonSerializer.Serialize(carts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de sauvegarder les paniers en attente : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class PendingCartStorage
        {
            public string CartNumber { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalWithoutTax { get; set; }
            public decimal TotalTTC { get; set; }
            public List<PendingCartItem> Items { get; set; } = new List<PendingCartItem>();
        }

        private class PendingCartItem
        {
            public int IdProduct { get; set; }
            public string RefProduct { get; set; }
            public string Designation { get; set; }
            public string CodeBarre { get; set; }
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal QtePack { get; set; }
            public string Taxe { get; set; }
            public decimal TarificationValue { get; set; }
            public decimal TotalWithoutTax { get; set; }
        }

        public class PriceOption
        {
            public string Display { get; set; }
            public decimal? Value { get; set; }
        }
        public bool CheckdgvArticlesRows()
        {

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                var RefProduct = row.Cells["Ref"].Value?.ToString();
                var Designation = row.Cells["Designation"].Value?.ToString();
                var Qte = Convert.ToDecimal(row.Cells["Qte"].Value?.ToString());

                if (string.IsNullOrWhiteSpace(RefProduct) && string.IsNullOrWhiteSpace(Designation))
                {
                    MessageBox.Show(
                        "Veuillez remplir la référence ou la désignation de tous les articles!",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }
                // Vérification du stock
                var product = _appContext.Products
              .FirstOrDefault(x => x.Id == Convert.ToInt32(row.Cells["IdProduct"].Value));
                if (product.StockQuantity < Qte)
                {
                    MessageBox.Show(
                      $"Quantité insuffisante en stock pour le produit : {Designation}",
                      "Validation",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
                    return false;
                }


                }
            if (dgvCart.Rows.Count == 0 || string.IsNullOrWhiteSpace(txtTicketNumber.Text))
            {
                MessageBox.Show(
                    "Veuillez ajouter au moins un article!",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }


            return true;
        }
    }
}
