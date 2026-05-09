using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public class SalePurchaseForm : Form
    {
        private readonly IService<SalesInvoicesDto> _service;
 
        private readonly FunctionUI _functionUI;
        private TextBox cmbClient;
        private TextBox txtNumFacture;
        private DateTimePicker dtpDate;
        private DataGridView dgvArticles;
        private Button btnAddRow;
        private ComboBox cbxCaisse;
        private NumericUpDown numTotalHT, numRemise, numTotalHTRemise, numTotalTVA, numTotalTTC, numMontantPaye, numResteAPayer;


        private AutoCompleteStringCollection refCollection = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection descCollection = new AutoCompleteStringCollection();
        public SalePurchaseForm()
        {
       
            InitializeCustomComponents();
            BtnAddRow_Click(null, null);
        }

     

        private void InitializeCustomComponents()
        {
            // Load autocomplete data from database
            LoadAutocompleteData();

            this.Text = "Nouvelle Vente";
            this.Size = new Size(1250, 850);
            this.StartPosition = FormStartPosition.CenterParent;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;

            // Main container
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(mainPanel);

            int yPosition = 10;

            // Header
            Label lblHeader = new Label
            {
                Text = "💰 NOUVELLE VENTE",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, yPosition)
            };
            mainPanel.Controls.Add(lblHeader);
            yPosition += 50;

            // ========== ENTÊTE SECTION ==========
            Panel entetePanel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(1120, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(entetePanel);

            Label lblEntete = new Label
            {
                Text = "Entête",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(10, 5),
                AutoSize = true
            };
            entetePanel.Controls.Add(lblEntete);

            // Client
            Label lblClient = new Label
            {
                Text = "Client:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 35),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            entetePanel.Controls.Add(lblClient);


            cmbClient = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(110, 35),
                Size = new Size(250, 25)
            };
            entetePanel.Controls.Add(cmbClient);

            using (var db = new AppDbContext())
            {
                var customerNames = db.Customers
                                      .Select(c => c.Name)
                                      .ToArray();
                var source = new AutoCompleteStringCollection();
                source.AddRange(customerNames);
                cmbClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbClient.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cmbClient.AutoCompleteCustomSource = source;
            }



            // N° Facture
            Label lblNumFacture = new Label
            {
                Text = "N°Facture:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(400, 35),
                Size = new Size(90, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            entetePanel.Controls.Add(lblNumFacture);

            txtNumFacture = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(500, 35),
                Size = new Size(200, 25),
                Text = "FAC-" + DateTime.Now.ToString("HHmmss")
            };
            entetePanel.Controls.Add(txtNumFacture);

            // Date
            Label lblDate = new Label
            {
                Text = "Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(750, 35),
                Size = new Size(50, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            entetePanel.Controls.Add(lblDate);

            dtpDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(810, 35),
                Size = new Size(250, 25),
                Format = DateTimePickerFormat.Short
            };
            entetePanel.Controls.Add(dtpDate);

            // Separator line
            Panel separator1 = new Panel
            {
                Location = new Point(10, 70),
                Size = new Size(1100, 2),
                BackColor = Color.FromArgb(189, 195, 199)
            };
            entetePanel.Controls.Add(separator1);

            yPosition += 110;

            // ========== DATAGRIDVIEW SECTION ==========
            Label lblArticles = new Label
            {
                Text = "Articles",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(10, yPosition),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblArticles);
            yPosition += 30;

            // Add Row Button
            btnAddRow = new Button
            {
                Text = "+",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 40),
                Location = new Point(10, yPosition),
                Cursor = Cursors.Hand
            };
            btnAddRow.FlatAppearance.BorderSize = 0;
            btnAddRow.Click += BtnAddRow_Click;
            mainPanel.Controls.Add(btnAddRow);

            // DataGridView
            // ========== DATAGRIDVIEW SECTION (MODIFIED) ==========
            dgvArticles = new DataGridView
            {
                Location = new Point(60, yPosition),
                Size = new Size(1200, 300),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                RowTemplate = { Height = 25 },
                ColumnHeadersHeight = 30
            };

            dgvArticles.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 11)
            };

            dgvArticles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvArticles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArticles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvArticles.EnableHeadersVisualStyles = false;


            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Réf Produit", Name = "RefProduit" });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Désignation", Name = "Designation", Width = 300 });
            dgvArticles.Columns.Add(new DataGridViewComboBoxColumn { HeaderText = "Tarification", Name = "Tarification", Width = 150, FlatStyle = FlatStyle.Flat });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Qte", Name = "Qte" });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Prix", Name = "Prix" });

            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Qte Pack", Name = "QtePack" });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TVA", Name = "TVA", ReadOnly = true });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total HT", Name = "TotalHT", ReadOnly = true });

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
            {
                HeaderText = "-",
                Name = "Delete",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 50
            };
            dgvArticles.Columns.Add(btnDelete);
            dgvArticles.CellContentClick += DgvArticles_CellContentClick;
            dgvArticles.CellValueChanged += DgvArticles_CellValueChanged;
            dgvArticles.CellEndEdit += DgvArticles_CellEndEdit;
            dgvArticles.EditingControlShowing += DgvArticles_EditingControlShowing;
            mainPanel.Controls.Add(dgvArticles);

            yPosition += 300;

            // Separator line
            Panel separator2 = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(1120, 2),
                BackColor = Color.FromArgb(189, 195, 199)
            };
            mainPanel.Controls.Add(separator2);
            yPosition += 10;

            // ========== PIED SECTION ==========
            Label lblPied = new Label
            {
                Text = "Pied",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(10, yPosition),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblPied);
            yPosition += 30;

            Panel piedPanel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(1120, 200),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(piedPanel);



            int piedY = 10;
            int labelWidth = 150;
            int fieldWidth = 200;

            // Total HT
          
            AddPiedField(piedPanel, "Total HT:", ref piedY, labelWidth, fieldWidth, out numTotalHT);
            numTotalHT.ReadOnly = true;
            numTotalHT.BackColor = Color.FromArgb(236, 240, 241);

            // Remise
           AddPiedField(piedPanel, "Remise:", ref piedY, labelWidth, fieldWidth, out numRemise);
            numRemise.ValueChanged += CalculateTotals;

            // Total HT Remisé
            AddPiedField(piedPanel, "Total HT Remisé:", ref piedY, labelWidth, fieldWidth, out numTotalHTRemise);
            numTotalHTRemise.ReadOnly = true;
            numTotalHTRemise.BackColor = Color.FromArgb(236, 240, 241);

            // Total TVA
            AddPiedField(piedPanel, "Total TVA:", ref piedY, labelWidth, fieldWidth, out numTotalTVA);
            numTotalTVA.ReadOnly = true;
            numTotalTVA.BackColor = Color.FromArgb(236, 240, 241);

            // Total TTC
            AddPiedField(piedPanel, "Total TTC:", ref piedY, labelWidth, fieldWidth, out numTotalTTC);
            numTotalTTC.ReadOnly = true;
            numTotalTTC.BackColor = Color.FromArgb(236, 240, 241);
            numTotalTTC.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            piedY = 10;
            int rightColumnX = 600;

            // Montant Payé
            AddPiedFieldRight(piedPanel, "Montant Payé:", ref piedY, rightColumnX, labelWidth, fieldWidth, out numMontantPaye);
            numMontantPaye.ValueChanged += CalculateTotals;

            // Reste à Payer
            AddPiedFieldRight(piedPanel, "Reste à Payer:", ref piedY, rightColumnX, labelWidth, fieldWidth, out numResteAPayer);
            numResteAPayer.ReadOnly = true;
            numResteAPayer.BackColor = Color.FromArgb(255, 235, 235);
            numResteAPayer.ForeColor = Color.FromArgb(192, 57, 43);
            numResteAPayer.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            AddPiedFieldRight(piedPanel, "Caise:", ref piedY, rightColumnX, labelWidth, fieldWidth, out cbxCaisse);

            yPosition += 210;




            // ========== BUTTONS ==========
            Panel buttonPanel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(1120, 60),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(buttonPanel);

            Button btnSave = new Button
            {
                Text = "💾 Enregistrer",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 50),
                Location = new Point(750, 5),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            buttonPanel.Controls.Add(btnSave);

            Button btnCancel = new Button
            {
                Text = "❌ Annuler",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 50),
                Location = new Point(940, 5),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            buttonPanel.Controls.Add(btnCancel);
        }

        private void DgvArticles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArticles.Rows[e.RowIndex];
            string colName = dgvArticles.Columns[e.ColumnIndex].Name;

            using (var db = new AppDbContext())
            {
                Product product = null;

                if (colName == "RefProduit")
                {
                    string val = row.Cells["RefProduit"].Value?.ToString();
                    product = db.Products.FirstOrDefault(p => p.RefProduct == val);
                }
                else if (colName == "Designation")
                {
                    string val = row.Cells["Designation"].Value?.ToString();
                    product = db.Products.FirstOrDefault(p => p.Designation == val);
                }
                else if (colName == "Tarification")
                {
                    if (row.Cells["Tarification"].Value != null)
                    {
                        if (decimal.TryParse(row.Cells["Tarification"].Value.ToString(), out decimal p))
                        {
                            row.Cells["Prix"].Value = p.ToString("N2");
                        }
                    }
                }

                if (product != null)
                {

                    dgvArticles.CellValueChanged -= DgvArticles_CellValueChanged;

                    row.Cells["RefProduit"].Value = product.RefProduct;
                    row.Cells["Designation"].Value = product.Designation;

                    // Populate Tarification
                    try
                    {
                        var priceLists = db.PriceLists.Where(pl => pl.Id == product.Id).ToList();
                        var options = new System.Collections.Generic.List<PriceOption>();
                        options.Add(new PriceOption { Display = $"Standard ({product.SalesPrice:N2})", Value = product.SalesPrice });
                        foreach (var pl in priceLists)
                        {
                            options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N2})", Value = pl.Price });
                        }

                        var tarifCell = (DataGridViewComboBoxCell)row.Cells["Tarification"];
                        tarifCell.DataSource = options;
                        tarifCell.DisplayMember = "Display";
                        tarifCell.ValueMember = "Value";
                        tarifCell.Value = product.SalesPrice;
                    }
                    catch { }

                    row.Cells["Prix"].Value = product.SalesPrice?.ToString("N2") ?? "0.00";
                    row.Cells["TVA"].Value = product.Taxe.ToString();
                    row.Cells["Qte"].Value = "1";

                    dgvArticles.CellValueChanged += DgvArticles_CellValueChanged;
                }
            }


            UpdateRowTotal(row);
            CalculateTotals(null, null);
        }
        public class PriceOption
        {
            public string Display { get; set; }
            public decimal? Value { get; set; }
        }
        private void CalculateTotals(object sender, EventArgs e)
        {
            decimal totalHT = 0;
            decimal totalTVA = 0;

            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.Cells["TotalHT"].Value != null)
                {
                    decimal rowTotalHT = 0;
                    decimal.TryParse(row.Cells["TotalHT"].Value.ToString(), out rowTotalHT);
                    totalHT += rowTotalHT;

                    // Calculate TVA for this row
                    string tvaStr = row.Cells["TVA"].Value?.ToString() ?? "0%";
                    decimal tvaPercent = 0;
                    if (tvaStr.Contains("%"))
                    {
                        decimal.TryParse(tvaStr.Replace("%", ""), out tvaPercent);
                    }
                    totalTVA += rowTotalHT * (tvaPercent / 100);
                }

            }

            numTotalHT.Value = totalHT;

            // Apply discount
            decimal remise = numRemise.Value;
            decimal totalHTRemise = totalHT - remise;
            numTotalHTRemise.Value = totalHTRemise > 0 ? totalHTRemise : 0;

            // Calculate TVA AFTER discount is applied
            totalTVA = 0;
            decimal baseForTVA = totalHTRemise > 0 ? totalHTRemise : totalHT;

            // Calculate proportional TVA for each row based on discounted amount
            if (totalHT > 0) // Avoid division by zero
            {
                foreach (DataGridViewRow row in dgvArticles.Rows)
                {
                    if (row.Cells["TotalHT"].Value != null)
                    {
                        decimal rowTotalHT = 0;
                        decimal.TryParse(row.Cells["TotalHT"].Value.ToString(), out rowTotalHT);

                        // Calculate this row's proportion of the total
                        decimal proportion = rowTotalHT / totalHT;
                        decimal rowBaseForTVA = baseForTVA * proportion;

                        // Get TVA percentage for this row
                        string tvaStr = row.Cells["TVA"].Value?.ToString() ?? "0%";
                        decimal tvaPercent = 0;
                        if (tvaStr.Contains("%"))
                        {
                            decimal.TryParse(tvaStr.Replace("%", ""), out tvaPercent);
                        }

                        // Add this row's TVA to the total
                        totalTVA += rowBaseForTVA * (tvaPercent / 100);
                    }
                }
            }

            numTotalTVA.Value = totalTVA;

            // Calculate total TTC
            decimal totalTTC = baseForTVA + totalTVA;
            numTotalTTC.Value = totalTTC;

            decimal montantPaye = numMontantPaye.Value;
            decimal resteAPayer = totalTTC - montantPaye;
            numResteAPayer.Value = resteAPayer > 0 ? resteAPayer : 0;
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
                DecimalPlaces = 2,
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
                DecimalPlaces = 2,
                ThousandsSeparator = true
            };
            parent.Controls.Add(numericUpDown);

            yPos += 30;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DgvArticles_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox autoText)
            {
                string colName = dgvArticles.CurrentCell.OwningColumn.Name;

                autoText.AutoCompleteMode = AutoCompleteMode.None; // Reset

                if (colName == "RefProduit" || colName == "Designation")
                {
                    autoText.AutoCompleteCustomSource = (colName == "RefProduit") ? refCollection : descCollection;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    autoText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }
        }

        private void DgvArticles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvArticles.Rows[e.RowIndex];
            string colName = dgvArticles.Columns[e.ColumnIndex].Name;

            // When user finishes editing Qte or Prix, update calculations
            if (colName == "Qte" || colName == "Prix")
            {
                UpdateRowTotal(row);
                CalculateTotals(null, null);
            }
            else if (colName == "QtePack")
            {
                CalculateQteFromPack(e.RowIndex);
                CalculateTotals(null, null);
            }
        }
        private void UpdateRowTotal(DataGridViewRow row)
        {
            if (row.Cells["Qte"].Value != null && row.Cells["Prix"].Value != null)
            {
                decimal qte = decimal.TryParse(row.Cells["Qte"].Value.ToString(), out var q) ? q : 0;
                decimal prix = decimal.TryParse(row.Cells["Prix"].Value.ToString(), out var p) ? p : 0;
                row.Cells["TotalHT"].Value = (qte * prix).ToString("N2");
            }
        }
        private void CalculateQteFromPack(int rowIndex)
        {
            try
            {
                var row = dgvArticles.Rows[rowIndex];
                string refProduct = row.Cells["RefProduit"].Value?.ToString();
                object qtePackObj = row.Cells["QtePack"].Value;

                if (string.IsNullOrEmpty(refProduct) || qtePackObj == null) return;

                if (decimal.TryParse(qtePackObj.ToString(), out decimal qtePack))
                {
                    using (var db = new AppDbContext())
                    {
                        var product = db.Products.AsNoTracking().FirstOrDefault(p => p.RefProduct == refProduct);
                        if (product != null && product.Colisage.HasValue && product.Colisage.Value > 0)
                        {
                            row.Cells["Qte"].Value = (qtePack * product.Colisage.Value).ToString("G29");
                            UpdateRowTotal(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log error
                MessageBox.Show("Erreur lors du calcul de la quantité colisage: " + ex.Message);
            }
        }

        private void DgvArticles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvArticles.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dgvArticles.Rows.RemoveAt(e.RowIndex);
                CalculateTotals(null, null);
            }
        }

      

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            // Ref, Desig, Tarif, Qte, Prix, QtePack, TVA, TotalHT
            int rowIndex = dgvArticles.Rows.Add("", "", null, "1", "0.00", "0", "0%", "0.00");
            dgvArticles.CurrentCell = dgvArticles.Rows[rowIndex].Cells["RefProduit"];
            dgvArticles.BeginEdit(true);
        }

        public void LoadAutocompleteData()
        {
            using (var db = new AppDbContext())
            {

                var products = db.Products
                        .Select(p => new { p.RefProduct, p.Designation })
                        .AsNoTracking()
                        .ToList();

                foreach (var p in products)
                {
                    if (!string.IsNullOrEmpty(p.RefProduct)) refCollection.Add(p.RefProduct);
                    if (!string.IsNullOrEmpty(p.Designation)) descCollection.Add(p.Designation);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        
        {
            if (keyData == Keys.Enter && dgvArticles.Focused && dgvArticles?.CurrentCell != null)
            {
                try
                {
                    var cell = dgvArticles.CurrentCell;
                    if (cell == null) return base.ProcessCmdKey(ref msg, keyData);

                    string colName = dgvArticles.Columns[cell.ColumnIndex].Name;
                    int row = cell.RowIndex;

                    // If editing, commit the edit so CellValueChanged fires and values are usable
                    if (dgvArticles.IsCurrentCellInEditMode)
                    {
                        dgvArticles.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        dgvArticles.EndEdit(); // Ensure the edit is fully committed

                        // For Qte and Prix columns, update calculations immediately
                        if (colName == "Qte" || colName == "Prix")
                        {
                            UpdateRowTotal(dgvArticles.Rows[row]);
                            CalculateTotals(null, null);
                        }
                    }

                    // Custom Navigation Logic
                    if (colName == "RefProduit" || colName == "Designation" || colName == "Tarification")
                    {
                        dgvArticles.CurrentCell = dgvArticles.Rows[row].Cells["Qte"];
                        dgvArticles.BeginEdit(true);
                    }
                    else if (colName == "Qte")
                    {
                        dgvArticles.CurrentCell = dgvArticles.Rows[row].Cells["Prix"];
                        dgvArticles.BeginEdit(true);
                    }
                    else if (colName == "Prix")
                    {
                        // dgvArticles.CurrentCell = dgvArticles.Rows[row].Cells["QtePack"];
                        BtnAddRow_Click(null, null);
                        dgvArticles.BeginEdit(true);
                    }
                    else if (colName == "QtePack")
                    {
                        CalculateQteFromPack(row);
                        int nextRow = row + 1;
                        if (nextRow >= dgvArticles.Rows.Count)
                        {
                            BtnAddRow_Click(null, null);
                        }
                        else
                        {
                            dgvArticles.CurrentCell = dgvArticles.Rows[nextRow].Cells["RefProduit"];
                            dgvArticles.BeginEdit(true);
                        }
                        CalculateTotals(null, null);
                    }

                    return true; // Handled
                }
                catch
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
