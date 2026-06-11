using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Services;
using InventoryManagement.UI.Achat;
using InventoryManagement.UI.Client;
using InventoryManagement.UI.Vente;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Fournisseur
{
    public partial class ModifierFournisseur : Form
    {
        private readonly FunctionUI _functionUI;
        private readonly IService<VendorDto> _service;
        private readonly IService<PaymentVendorDto> _servicePay;
        private readonly IService<PurchaseDto> _purchaseService;
        private readonly IFormManager _formFactory;
        private readonly AppDbContext _appContext;

        private int _id;

        private TextBox txtRef;
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private TextBox txtRemark;
        private Label lblBalance;   // Solde (Highlighted)
        private Label lblTurnover;  // Chiffre d'affaire

        // Grids
        private DataGridView dgvSales;
        private DataGridView dgvPayments;
        private Button btnNewPayment;
        public ModifierFournisseur(
            int id, FunctionUI functionUI, IService<VendorDto> service,
            IService<PaymentVendorDto> servicePay, IService<PurchaseDto> purchaseService,
            IFormManager formManager, AppDbContext appDbContext)
        {
            _id = id;
            _functionUI = functionUI;
            _service = service;
            _servicePay = servicePay;
            _appContext = appDbContext;
            _purchaseService = purchaseService;
            _formFactory = formManager;
            InitializeComponent();
            InitializeCustomComponents();
           LoadData(_id);
        }

        private void InitializeCustomComponents()
        {
            this.Text = "FICHE FOURNISSEUR";
            this.Size = new Size(1250, 850);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Main Layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6, // Header, Details, LabelSales, GridSales, LabelPayments, GridPayments
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Header Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F)); // Customer Details Section
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Label Sales
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Grid Sales
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Label Payments
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Grid Payments

            this.Controls.Add(mainLayout);

            // 1. Header Title
            Label lblHeader = new Label
            {
                Text = "  👤 FICHE FOURNISSEUR",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113), // Green theme
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.Controls.Add(lblHeader, 0, 0);

            // 2. Customer Details Section (Two columns: Left Info, Right Financials)
            Panel detailsPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };

            // We use a GroupBox or just a layout inside detailsPanel
            TableLayoutPanel detailsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F)); // General Info
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Financial Info

            // Left Side: General Info
            Panel leftInfoPanel = new Panel { Dock = DockStyle.Fill };
            InitializeGeneralInfo(leftInfoPanel);

            // Right Side: Financial Info (Solde, CA)
            Panel rightInfoPanel = new Panel { Dock = DockStyle.Fill };
            InitializeFinancialInfo(rightInfoPanel);

            detailsLayout.Controls.Add(leftInfoPanel, 0, 0);
            detailsLayout.Controls.Add(rightInfoPanel, 1, 0);

            detailsPanel.Controls.Add(detailsLayout);
            mainLayout.Controls.Add(detailsPanel, 0, 1);


            // 3. Label History Sales
            Label lblHistorySales = new Label
            {
                Text = "Historique des Ventes",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(0, 0, 0, 5)
            };
            mainLayout.Controls.Add(lblHistorySales, 0, 2);

            // 4. Grid Sales
            dgvSales = CreateCustomDataGridView();
            SetupSalesColumns(dgvSales);
            mainLayout.Controls.Add(dgvSales, 0, 3);

            // 5. Payments Header (Label + Button)
            Panel pnlPaymentsHeader = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };

            Label lblHistoryPayments = new Label
            {
                Text = "Historique des Versements",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(0, 0, 0, 5),
                AutoSize = true
            };

            btnNewPayment = new Button
            {
                Text = "+ Nouveau Versement",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113), // Green to match header
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 30), // Height fits within row roughly
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                Margin = new Padding(0, 5, 0, 0) // slight top margin if needed
            };
            btnNewPayment.FlatAppearance.BorderSize = 0;
            btnNewPayment.Click += BtnNewPayment_Click;

            pnlPaymentsHeader.Controls.Add(lblHistoryPayments);
            pnlPaymentsHeader.Controls.Add(btnNewPayment);
            mainLayout.Controls.Add(pnlPaymentsHeader, 0, 4);

            // 6. Grid Payments
            dgvPayments = CreateCustomDataGridView();
            SetupPaymentsColumns(dgvPayments);
            mainLayout.Controls.Add(dgvPayments, 0, 5);


            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.MultiSelect = false;
            dgvSales.ReadOnly = true;
            dgvSales.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgvSales.Columns[0].Name = "ID";
            dgvSales.CellDoubleClick += DgvSales_CellDoubleClick;
        }
        public async Task LoadData(int id)
        {
            var vendor = await _service.GetAsyncById(id);

            if (vendor == null)
                return;

            txtRef.Text = vendor.RefVendor;
            txtName.Text = vendor.Name;
            txtPhone.Text = vendor.PhoneNumber;
            txtAddress.Text = vendor.Address;
            txtRemark.Text = vendor.Remark; 
            lblBalance.Text = vendor.Balance?.ToString("C2") ?? "0.00";
            lblTurnover.Text = vendor.Turnover?.ToString("C2") ?? "0.00";

            // Vider le DataGridView avant remplissage
            dgvSales.Rows.Clear();

            var purchase = (await _purchaseService.GetAllAsyncs())
                .Where(x => x.IdVendor == id)
                .ToList();
            if (purchase != null)
            {
                foreach (var sale in purchase)
                {
                    dgvSales.Rows.Add(
                        sale.Id,
                        sale.NumberPurchase,
                        sale.DatePurchase.ToShortDateString(),
                        (sale.TotalPurchase ?? 0).ToString("N2"),
                        (sale.PaymentPurchase ?? 0).ToString("N2"),
                        (sale.BalancePurchase ?? 0).ToString("N2")
                    );
                }
            }
            var payments = (await _servicePay.GetAllAsyncs())
                .Where(x => x.IdVendor == id)
                .ToList();

            dgvPayments.Rows.Clear();
            if (payments != null) { 
                foreach (var payment in payments)
            {
                var crate = await _appContext.Crates
        .FirstOrDefaultAsync(c => c.Id == payment.IdCrates);
                dgvPayments.Rows.Add(
                    payment.NumberPayment,
                    payment.DatePayment.ToShortDateString(),
                    (payment.Payment ?? 0).ToString("N2"),
                     crate?.Name ?? ""

                );
            }
            }
        }

        private void InitializeGeneralInfo(Panel panel)
        {
            // Simple layout for textboxes
            int startY = 0;
            int gapY = 30;
            int labelWidth = 100;
            int textWidth = 300;

            // Ref
            panel.Controls.Add(CreateLabel("Référence:", 10, startY + 5));
            txtRef = CreateEditableTextBox(120, startY, textWidth);
            panel.Controls.Add(txtRef);

            // Nom
            startY += gapY;
            panel.Controls.Add(CreateLabel("Nom:", 10, startY + 5));
            txtName = CreateEditableTextBox(120, startY, textWidth);
            panel.Controls.Add(txtName);

            // Phone
            startY += gapY;
            panel.Controls.Add(CreateLabel("N° Téléphone:", 10, startY + 5));
            txtPhone = CreateEditableTextBox(120, startY, textWidth);
            panel.Controls.Add(txtPhone);

            // Address
            startY += gapY;
            panel.Controls.Add(CreateLabel("Adresse:", 10, startY + 5));
            txtAddress = CreateEditableTextBox(120, startY, textWidth);
            panel.Controls.Add(txtAddress);

            // Remark (Multiline potentially, or single line)
            startY += gapY;
            panel.Controls.Add(CreateLabel("Remarque:", 10, startY + 5));
            txtRemark = CreateEditableTextBox(120, startY, textWidth);
            panel.Controls.Add(txtRemark);

            // Save Button
            startY += gapY;
            Button btnSaveClient = new Button
            {
                Text = "💾 Enregistrer",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 30),
                Location = new Point(120, startY),
                Cursor = Cursors.Hand
            };
            btnSaveClient.FlatAppearance.BorderSize = 0;
            btnSaveClient.Click += BtnSaveClient_Click;
            panel.Controls.Add(btnSaveClient);
        }
        private void InitializeFinancialInfo(Panel panel)
        {
            // Big labels for Solde and CA
            int startY = 10;

            // Solde Box
            Panel pnlSolde = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(300, 70),
                BackColor = Color.FromArgb(231, 76, 60) // Reddish for debt/balance usually, or neutral
            };
            Label lblSoldeTitle = new Label { Text = "SOLDE", ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Regular), Location = new Point(10, 5), AutoSize = true };
            lblBalance = new Label { Text = "0.00", ForeColor = Color.White, Font = new Font("Consolas", 20, FontStyle.Bold), Location = new Point(10, 25), AutoSize = true };
            pnlSolde.Controls.Add(lblSoldeTitle);
            pnlSolde.Controls.Add(lblBalance);
            panel.Controls.Add(pnlSolde);

            // CA Box
            startY += 80;
            Panel pnlCA = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(300, 70),
                BackColor = Color.FromArgb(52, 152, 219) // Blue
            };
            Label lblCATitle = new Label { Text = "CHIFFRE D'AFFAIRE", ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Regular), Location = new Point(10, 5), AutoSize = true };
            lblTurnover = new Label { Text = "0.00", ForeColor = Color.White, Font = new Font("Consolas", 20, FontStyle.Bold), Location = new Point(10, 25), AutoSize = true };
            pnlCA.Controls.Add(lblCATitle);
            pnlCA.Controls.Add(lblTurnover);
            panel.Controls.Add(pnlCA);
        }
        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.DimGray
            };
        }
        private TextBox CreateEditableTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 25),
                ReadOnly = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle
            };
        }
        private DataGridView CreateCustomDataGridView()
        {
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            //dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 240, 241);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.RowTemplate.Height = 30;
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }
        private void SetupSalesColumns(DataGridView dgv)
        {
            dgv.Columns.Add("ID", "ID");
            dgv.Columns.Add("N_Facture", "N_Facture");
            dgv.Columns.Add("Date", "Date");
            dgv.Columns.Add("Montant_TTC", "Montant_TTC");
            dgv.Columns.Add("Payé", "Payé");
            dgv.Columns.Add("Reste", "Reste");
            dgv.Columns["ID"].Visible = false; // Masquer la colonne ID
        }

        private async void BtnNewPayment_Click(object? sender, EventArgs e)
        {
            _formFactory.Open<AjouterPaymentFournisseur>(_id);
            await LoadData(_id);
        }
        private void SetupPaymentsColumns(DataGridView dgv)
        {
            dgv.Columns.Add("N_Versement", "N_Versement");
            dgv.Columns.Add("Date", "Date");
            dgv.Columns.Add("Montant", "Montant");
            dgv.Columns.Add("Caisse", "Caisse");
        }
        private async void DgvSales_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvSales.SelectedRows.Count == 0) return;
                var row = dgvSales.SelectedRows[0];
                if (row.Cells["ID"].Value == null) return;
                int id = Convert.ToInt32(row.Cells["ID"].Value);
                _formFactory.Open<ModifierAchat>(id);
                await LoadData(_id);
            }
        }
        private async void BtnSaveClient_Click(object? sender, EventArgs e)
        {
            try
            {
                var vendorDto = new VendorDto
                {
                    RefVendor = txtRef.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Remark = txtRemark.Text.Trim()
                };

                await _service.UpdateAsync(vendorDto, _id);
                MessageBox.Show("Fournisseur mis à jour avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
               await LoadData(_id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
