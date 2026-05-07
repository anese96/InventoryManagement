using InventoryManagement.Data.DTO;
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

namespace InventoryManagement.UI.Client
{
    public partial class AjouterClient : Form
    {
        private readonly IService<ClientDto> _service;
        private readonly FunctionUI _functionUI;

        private TextBox txtReference, txtNom, txtTelephone, txtAdresse, txtRemarque;
        public AjouterClient(FunctionUI functionUI , IService<ClientDto> service)
        {
            InitializeComponent();
            _functionUI = functionUI;
            InitializeCustomComponents();
            _service = service;
           
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Ajouter un Client";
            this.Size = new Size(700, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(mainLayout);

            // Header
            Label lblHeader = new Label
            {
                Text = "👤 AJOUTER UN CLIENT",
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

            _functionUI.AddFormField(formPanel, "Référence :", lblW, fldW, 30, spc, out txtReference, "", "", false);
            txtReference.Text = "CL-" + DateTime.Now.ToString("HHmmss");
            _functionUI.AddFormField(formPanel, "Nom :", lblW, fldW, 30, spc, out txtNom, "", "", false);
            _functionUI.AddFormField(formPanel, "N° Téléphone :", lblW, fldW, 30, spc, out txtTelephone, "", "", false);
            _functionUI.AddFormField(formPanel, "Adresse :", lblW, fldW, 30, spc, out txtAdresse, "", "", false);
            _functionUI.AddFormField(formPanel, "Remarque :", lblW, fldW, 30, spc, out txtRemarque, "", "", false);

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
                var clientDto = new ClientDto
                {
                    RefCustomer = txtReference.Text.Trim(),
                    Name = txtNom.Text.Trim(),
                    PhoneNumber = txtTelephone.Text.Trim(),
                    Address = txtAdresse.Text.Trim(),
                    Remark = txtRemarque.Text.Trim(),
                };
                await _service.AddAsync(clientDto);
              

                MessageBox.Show("Client ajouté avec succès");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
                this.DialogResult = DialogResult.OK;
            }

        }
    }
}
