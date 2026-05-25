using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
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

namespace InventoryManagement.UI.Client
{
    public partial class AjouterPayment : Form
    {
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appContext;

        private readonly IService<PaymentCustomerDto> _service;
        private int _idCustomer;

        private TextBox txtNumberPayment, txtAmount;
        private DateTimePicker dtpDate;
        private ComboBox cbCaisse;
        private readonly ClientService _clientService;

        public AjouterPayment(int idCustomer, FunctionUI functionUI, IService<PaymentCustomerDto> service, AppDbContext appDbContext, ClientService clientService)
        {
            _idCustomer = idCustomer;
             _functionUI = functionUI;
             _service = service;
             _appContext = appDbContext;
             _clientService = clientService;
            InitializeComponent();
            InitializeCustomComponents();
        }
        private void InitializeCustomComponents()
        {
            this.Text = "Ajouter un Paiement";
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            this.Controls.Add(mainLayout);

            // Header
            Label lblHeader = new Label
            {
                Text = "👤 AJOUTER UN VERSEMENT",
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

            _functionUI.AddFormField(formPanel, "N° Versement :", lblW, fldW, 30, spc, out txtNumberPayment, "", "", false);
            txtNumberPayment.Text = "PYM-" + DateTime.Now.ToString("HHmmss");
            // Date
            Panel pnlDate = new Panel { Size = new Size(lblW + fldW + 20, 35), Margin = new Padding(0, 0, 0, spc) };

            Label lblDate = new Label
            {
                Text = "Date :",
                Size = new Size(lblW, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10)
            };

            dtpDate = new DateTimePicker
            {
                Size = new Size(fldW, 30),
                Location = new Point(lblW, 0),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };

            pnlDate.Controls.Add(lblDate);
            pnlDate.Controls.Add(dtpDate);
            formPanel.Controls.Add(pnlDate);


            _functionUI.AddFormField(formPanel, "Montant :", lblW, fldW, 30, spc, out txtAmount, "N2", "0,00");
            _functionUI.AddComboBoxField(formPanel, "Caisse:", lblW, fldW, spc, out cbCaisse);
          
                var crates = _appContext.Crates.ToList();
                cbCaisse.DataSource = crates;
                cbCaisse.DisplayMember = "Name";
                cbCaisse.ValueMember = "Id";

            
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 0, 25, 0),
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

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var clientDto = new PaymentCustomerDto
                {
                    IdCustomer = _idCustomer,
                    NumberPayment = txtNumberPayment.Text.Trim(),
                    DatePayment = dtpDate.Value,
                    Payment = decimal.Parse(txtAmount.Text.Trim()),
                    IdCrates = (int)cbCaisse.SelectedValue
                };
                await _service.AddAsync(clientDto);
                await _clientService.UpdateBalanceAsync(_idCustomer, -(clientDto.Payment.Value));

                MessageBox.Show("Payment ajouté avec succès");
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
    }
}
