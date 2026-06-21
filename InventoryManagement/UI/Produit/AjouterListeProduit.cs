using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace InventoryManagement.UI.Produit
{
    public partial class AjouterListeProduit : Form

    {
        private readonly IService<ProduitDto> _service;
        private DataGridView dgvArticles;
        private CheckBox cbAutoRef;
        private CheckBox cbAutcode;
        private readonly AppDbContext _appDbContext;

        private List<Category> _categories = new();
        private List<Unit> _units = new();
        private List<Marque> _marques = new();

        private HashSet<string> _dbReferences = new();
        private HashSet<string> _dbBarcodes = new();
        private HashSet<string> _dbDesignations = new();

        private readonly Color PrimaryColor = Color.FromArgb(52, 152, 219);
        private readonly Color SuccessColor = Color.FromArgb(46, 204, 113);
        private readonly Font HeaderFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Font StandardFont = new Font("Segoe UI", 10, FontStyle.Bold);

        public AjouterListeProduit(AppDbContext appDbContext , IService<ProduitDto> service)
        {

            _appDbContext = appDbContext;
            _service = service;
            InitializeComponent();
            InitializeCustomComponents();
            BtnAddRow_Click(null, null);
          
        }

        private void InitializeCustomComponents()
        {
            _categories = _appDbContext.Categories.OrderBy(c => c.Name).ToList();
            _units = _appDbContext.Units.OrderBy(u => u.Name).ToList();
            _marques = _appDbContext.Marques.OrderBy(m => m.Name).ToList();

            this.Text = "Ajouter list Produits";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
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
                Text = "📋 Ajouter list Produits",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, yPosition)
            };
            mainPanel.Controls.Add(lblHeader);
            yPosition += 55;

            int controlsY = yPosition;
            Button btnAddRow = CreateStyledButton("+", SuccessColor, new Size(40, 30), new Point(300, controlsY), 12);
            btnAddRow.Click += BtnAddRow_Click;
            mainPanel.Controls.Add(btnAddRow);

            cbAutoRef = CreateStyledCheckBox("Référence automatique", new Point(360, controlsY + 5), true);
            mainPanel.Controls.Add(cbAutoRef);

            cbAutcode = CreateStyledCheckBox("Code Barre automatique", new Point(560, controlsY + 5), true);
            mainPanel.Controls.Add(cbAutcode);

            // Add Excel buttons
            Button btnDownloadTemplate = CreateStyledButton("📥 Télécharger le modèle Excel", Color.FromArgb(41, 128, 185), new Size(240, 30), new Point(780, controlsY), 9);
            btnDownloadTemplate.Click += BtnDownloadTemplate_Click;
            mainPanel.Controls.Add(btnDownloadTemplate);

            Button btnImportExcel = CreateStyledButton("📤 Importer depuis Excel", Color.FromArgb(39, 174, 96), new Size(200, 30), new Point(1030, controlsY), 9);
            btnImportExcel.Click += BtnImportExcel_Click;
            mainPanel.Controls.Add(btnImportExcel);

            yPosition += 40;

            // DataGridView Configuration
            dgvArticles = new DataGridView
            {
                Name = "dgvArticles",
                Location = new Point(10, yPosition),
                Size = new Size(1200, 700), //new Size(mainPanel.Width - 40, mainPanel.Height - yPosition - 80),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = true,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Styling
            dgvArticles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvArticles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArticles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvArticles.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(44, 62, 80);
            dgvArticles.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvArticles.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvArticles.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvArticles.GridColor = Color.FromArgb(236, 240, 241);
            dgvArticles.EnableHeadersVisualStyles = false;
            dgvArticles.RowTemplate.Height = 25;

            // Columns definition
            int refIndex = dgvArticles.Columns.Add("Reference", "Référence");
            dgvArticles.Columns[refIndex].FillWeight = 100;
            int descIndex = dgvArticles.Columns.Add("Description", "Désignation");
            dgvArticles.Columns[descIndex].FillWeight = 300;
            dgvArticles.Columns.Add("BarCode", "Code Barre");
            
            var categoryColumn = new DataGridViewComboBoxColumn
            {
                Name = "Category",
                HeaderText = "Catégorie",
                DataSource = _categories,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat
            };
            dgvArticles.Columns.Add(categoryColumn);

            var marqueColumn = new DataGridViewComboBoxColumn
            {
                Name = "Marque",
                HeaderText = "Marque",
                DataSource = _marques,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat
            };
            dgvArticles.Columns.Add(marqueColumn);

            // Numeric Columns Style
            var numericStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight };
            var intStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight };

            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PurchasePrice", HeaderText = "Prix Achat", DefaultCellStyle = numericStyle });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "SalePrice", HeaderText = "Prix Vente", DefaultCellStyle = numericStyle });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "InitialStock", HeaderText = "Stock Initial", DefaultCellStyle = numericStyle });
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "StockAlert", HeaderText = "Alerte Stock", DefaultCellStyle = intStyle });
            
            var tvaColumn = new DataGridViewComboBoxColumn
            {
                Name = "TVA",
                HeaderText = "TVA",
                DefaultCellStyle = numericStyle,
                FlatStyle = FlatStyle.Flat
            };
            tvaColumn.Items.AddRange("0%", "9%", "19%");
            dgvArticles.Columns.Add(tvaColumn);

            var unitColumn = new DataGridViewComboBoxColumn
            {
                Name = "Unit",
                HeaderText = "Unité",
                DataSource = _units,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat
            };
            dgvArticles.Columns.Add(unitColumn);
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Packing", HeaderText = "Colisage", DefaultCellStyle = intStyle });

            // Add Error column
            var errStyle = new DataGridViewCellStyle { ForeColor = Color.Red, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
            int errIndex = dgvArticles.Columns.Add("Error", "Erreur");
            dgvArticles.Columns[errIndex].DefaultCellStyle = errStyle;
            dgvArticles.Columns[errIndex].ReadOnly = true;
            dgvArticles.Columns[errIndex].FillWeight = 200;

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "Delete",
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 30
            };
            dgvArticles.Columns.Add(btnDelete);
            
            dgvArticles.CellContentClick += DgvArticles_CellContentClick;
            dgvArticles.CellValueChanged += DgvArticles_CellValueChanged;

            mainPanel.Controls.Add(dgvArticles);


            Button btnCancel = CreateStyledButton("❌ Annuler", Color.FromArgb(231, 76, 60), new Size(150, 45), Point.Empty, 10);
            btnCancel.Location = new Point(mainPanel.Width - 340, dgvArticles.Bottom + 20);
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            mainPanel.Controls.Add(btnCancel);
            btnCancel.Click += BtnCancel_Click;

            Button btnSave = CreateStyledButton("💾 Enregistrer", Color.FromArgb(39, 174, 96), new Size(150, 45), Point.Empty, 12);
            btnSave.Location = new Point(mainPanel.Width - 170, dgvArticles.Bottom + 20);
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            mainPanel.Controls.Add(btnSave);
            btnSave.Click += async (s, e) => await BtnSave_Click(s, e);
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

                    // If editing, commit the edit
                    if (dgvArticles.IsCurrentCellInEditMode)
                    {
                        dgvArticles.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        dgvArticles.EndEdit();
                    }

                    // Navigation Logic
                    string[] colOrder = { "Reference", "Description", "BarCode", "Category", "Marque", "PurchasePrice", "SalePrice", "InitialStock", "StockAlert", "TVA", "Unit", "Packing" };
                    int currentIndex = Array.IndexOf(colOrder, colName);

                    if (currentIndex >= 0 && currentIndex < colOrder.Length - 1)
                    {
                        // Move to next column
                        string nextCol = colOrder[currentIndex + 1];
                        dgvArticles.CurrentCell = dgvArticles.Rows[row].Cells[nextCol];
                        dgvArticles.BeginEdit(true);
                    }
                    else if (colName == "Packing") // Last column
                    {
                        // Move to next row, first column
                        int nextRow = row + 1;
                        BtnAddRow_Click(null, null);
                        // Check if we can move to next row (it might exist if AllowUserToAddRows is true and we are not at the new row yet, or if it's already there)
                        if (nextRow < dgvArticles.Rows.Count)
                        {

                            dgvArticles.CurrentCell = dgvArticles.Rows[nextRow].Cells["Reference"];
                            dgvArticles.BeginEdit(true);
                        }
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

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            string refProduit = "";
            if (cbAutoRef != null && cbAutoRef.Checked)
            {
                refProduit = "PRO-" + DateTime.Now.ToString("HHmmssff");
            }

            string barCode = "";
            if (cbAutcode != null && cbAutcode.Checked)
            {
                barCode = GenerateBarcode();
            }

            int rowIndex = dgvArticles.Rows.Add(refProduit, "", barCode, null, null, "0.00", "0.00", "0.00", "0", "0%", null, "0");

            if (string.IsNullOrEmpty(refProduit))
            {
                dgvArticles.CurrentCell = dgvArticles.Rows[rowIndex].Cells["Reference"];
            }
            else
            {
                dgvArticles.CurrentCell = dgvArticles.Rows[rowIndex].Cells["Description"];
            }

            dgvArticles.BeginEdit(true);
        }

        private void DgvArticles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvArticles.Columns["Delete"].Index)
            {
                dgvArticles.Rows.RemoveAt(e.RowIndex);
            }
        }

        private async Task BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Force a validation run to make sure all errors are up to date
                ValidateAllRows();

                bool hasErrors = false;
                foreach (DataGridViewRow row in dgvArticles.Rows)
                {
                    if (row.IsNewRow) continue;
                    string err = row.Cells["Error"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(err))
                    {
                        hasErrors = true;
                        break;
                    }
                }

                if (hasErrors)
                {
                    MessageBox.Show("Veuillez corriger toutes les erreurs (lignes en rouge) avant d'enregistrer.", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int savedCount = 0;
                foreach (DataGridViewRow row in dgvArticles.Rows)
                {
                    if (row.IsNewRow) continue;

                    string designation = row.Cells["Description"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(designation)) continue;

                    var product = new ProduitDto
                    {
                        RefProduct = row.Cells["Reference"].Value?.ToString(),
                        Designation = designation,
                        BarCode = row.Cells["BarCode"].Value?.ToString(),
                        CategoryId = (row.Cells["Category"].Value is int idCat) ? (int?)idCat : null,
                        MarqueId = (row.Cells["Marque"].Value is int idMarque) ? (int?)idMarque : null,
                        PurchasePrice = ParseDecimal(row.Cells["PurchasePrice"].Value?.ToString()),
                        SalesPrice = ParseDecimal(row.Cells["SalePrice"].Value?.ToString()),
                        StockQuantity = ParseDecimal(row.Cells["InitialStock"].Value?.ToString()),
                        QtyAlert = ParseDecimal(row.Cells["StockAlert"].Value?.ToString()),
                        Taxe = row.Cells["TVA"].Value?.ToString(),
                        UnitId = (row.Cells["Unit"].Value is int idUnit) ? (int?)idUnit : null,
                        Colisage = (int?)ParseDecimal(row.Cells["Packing"].Value?.ToString())
                    };

                    await _service.AddAsync(product);
                    savedCount++;
                }

                if (savedCount > 0)
                {
                    MessageBox.Show($"{savedCount} produits enregistrés avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Aucun produit valide à enregistrer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDbCache()
        {
            _dbReferences = _appDbContext.Products.Select(p => p.RefProduct != null ? p.RefProduct.ToLower() : "").ToHashSet();
            _dbBarcodes = _appDbContext.Products.Where(p => !string.IsNullOrEmpty(p.BarCode)).Select(p => p.BarCode!).ToHashSet();
            _dbDesignations = _appDbContext.Products.Select(p => p.Designation.ToLower()).ToHashSet();
        }

        private void ValidateAllRows()
        {
            RefreshDbCache();

            var refCounts = new Dictionary<string, int>();
            var barcodeCounts = new Dictionary<string, int>();
            var designationCounts = new Dictionary<string, int>();

            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.IsNewRow) continue;

                string r = row.Cells["Reference"].Value?.ToString()?.Trim()?.ToLower();
                if (!string.IsNullOrEmpty(r))
                {
                    refCounts[r] = refCounts.GetValueOrDefault(r) + 1;
                }

                string b = row.Cells["BarCode"].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(b))
                {
                    barcodeCounts[b] = barcodeCounts.GetValueOrDefault(b) + 1;
                }

                string d = row.Cells["Description"].Value?.ToString()?.Trim()?.ToLower();
                if (!string.IsNullOrEmpty(d))
                {
                    designationCounts[d] = designationCounts.GetValueOrDefault(d) + 1;
                }
            }

            var duplicateReferences = refCounts.Where(kv => kv.Value > 1).Select(kv => kv.Key).ToHashSet();
            var duplicateBarcodes = barcodeCounts.Where(kv => kv.Value > 1).Select(kv => kv.Key).ToHashSet();
            var duplicateDesignations = designationCounts.Where(kv => kv.Value > 1).Select(kv => kv.Key).ToHashSet();

            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                ValidateRow(row, duplicateReferences, duplicateBarcodes, duplicateDesignations);
            }
        }

        private void ValidateRow(DataGridViewRow row, HashSet<string> duplicateReferences, HashSet<string> duplicateBarcodes, HashSet<string> duplicateDesignations)
        {
            if (row.IsNewRow) return;

            var errors = new List<string>();

            // 1. Designation obligatoire
            string designation = row.Cells["Description"].Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(designation))
            {
                errors.Add("Désignation obligatoire.");
            }
            else
            {
                string lowerDes = designation.ToLower();
                if (_dbDesignations.Contains(lowerDes))
                {
                    errors.Add("Désignation existante en BDD.");
                }
                if (duplicateDesignations.Contains(lowerDes))
                {
                    errors.Add("Désignation en double.");
                }
            }

            // 2. Reference obligatoire & unique
            string reference = row.Cells["Reference"].Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(reference))
            {
                errors.Add("Référence obligatoire.");
            }
            else
            {
                string lowerRef = reference.ToLower();
                if (_dbReferences.Contains(lowerRef))
                {
                    errors.Add("Référence existante en BDD.");
                }
                if (duplicateReferences.Contains(lowerRef))
                {
                    errors.Add("Référence en double.");
                }
            }

            // 3. CodeBarre unique
            string barcode = row.Cells["BarCode"].Value?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(barcode))
            {
                if (_dbBarcodes.Contains(barcode))
                {
                    errors.Add("Code-barres existant en BDD.");
                }
                if (duplicateBarcodes.Contains(barcode))
                {
                    errors.Add("Code-barres en double.");
                }
            }

            // 4. Prix >= 0
            decimal purchasePrice = ParseDecimal(row.Cells["PurchasePrice"].Value?.ToString()) ?? 0;
            decimal salePrice = ParseDecimal(row.Cells["SalePrice"].Value?.ToString()) ?? 0;
            if (purchasePrice < 0)
            {
                errors.Add("Prix achat négatif.");
            }
            if (salePrice < 0)
            {
                errors.Add("Prix vente négatif.");
            }

            // 5. Stock >= 0
            decimal initialStock = ParseDecimal(row.Cells["InitialStock"].Value?.ToString()) ?? 0;
            decimal stockAlert = ParseDecimal(row.Cells["StockAlert"].Value?.ToString()) ?? 0;
            if (initialStock < 0)
            {
                errors.Add("Stock initial négatif.");
            }
            if (stockAlert < 0)
            {
                errors.Add("Stock alerte négatif.");
            }

            // 6. TVA autorisée : 0, 9 ou 19
            string tva = row.Cells["TVA"].Value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(tva) || (tva != "0%" && tva != "9%" && tva != "19%"))
            {
                errors.Add("TVA doit être 0%, 9% ou 19%.");
            }

            // Apply style based on validation
            if (errors.Count > 0)
            {
                row.Cells["Error"].Value = string.Join(", ", errors);
                row.DefaultCellStyle.BackColor = Color.FromArgb(253, 237, 237); // Light pink
                row.DefaultCellStyle.ForeColor = Color.FromArgb(184, 15, 10);   // Dark red
            }
            else
            {
                row.Cells["Error"].Value = "";
                row.DefaultCellStyle.BackColor = Color.FromArgb(235, 247, 235); // Light green
                row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 115, 30);   // Dark green
            }
        }

        private void DgvArticles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ValidateAllRows();
            }
        }

        private string NormalizeTva(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "0%";
            string clean = input.Replace("%", "").Trim();
            if (decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
            {
                // Handle 0.19 -> 19%, 0.09 -> 9%
                if (val == 0.19m) return "19%";
                if (val == 0.09m) return "9%";
                if (val == 0m) return "0%";

                if (val == 19m) return "19%";
                if (val == 9m) return "9%";
                if (val == 0m) return "0%";
            }
            return input; // fallback
        }

        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichier Excel (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "Modele_Import_Produits.xlsx";
                saveFileDialog.Title = "Enregistrer le modèle Excel";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Produits");

                            string[] headers = {
                                "Référence", "Désignation", "CodeBarre", "Catégorie", "Marque",
                                "PrixAchat", "PrixVente", "StockInitial", "StockAlerte", "TVA", "Unité", "Colisage"
                            };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                var cell = worksheet.Cell(1, i + 1);
                                cell.Value = headers[i];
                                cell.Style.Font.Bold = true;
                                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#EAECEE");
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }

                            // Formats
                            worksheet.Column(6).Style.NumberFormat.Format = "#,##0.00"; // PrixAchat
                            worksheet.Column(7).Style.NumberFormat.Format = "#,##0.00"; // PrixVente
                            worksheet.Column(8).Style.NumberFormat.Format = "#,##0.00"; // StockInitial
                            worksheet.Column(9).Style.NumberFormat.Format = "#,##0";    // StockAlerte
                            worksheet.Column(12).Style.NumberFormat.Format = "#,##0";   // Colisage

                            // TVA drop-down list (columns J, rows 2 to 1000)
                            var rangeTva = worksheet.Range("J2:J1000");
                            var validation = rangeTva.CreateDataValidation();
                            validation.AllowedValues = XLAllowedValues.List;
                            validation.Value = "\"0%,9%,19%\"";
                            validation.IgnoreBlanks = true;
                            validation.ShowErrorMessage = true;
                            validation.ErrorTitle = "TVA Invalide";
                            validation.ErrorMessage = "Veuillez choisir une valeur autorisée : 0%, 9%, 19%.";

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(saveFileDialog.FileName);
                        }

                        MessageBox.Show("Modèle Excel téléchargé avec succès !", "Téléchargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la génération du modèle : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichier Excel (*.xlsx)|*.xlsx";
                openFileDialog.Title = "Importer depuis Excel";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook(openFileDialog.FileName))
                        {
                            if (!workbook.Worksheets.TryGetWorksheet("Produits", out var worksheet))
                            {
                                MessageBox.Show("La feuille nommée 'Produits' est introuvable dans le fichier Excel.", "Erreur d'import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string[] expectedHeaders = {
                                "Référence", "Désignation", "CodeBarre", "Catégorie", "Marque",
                                "PrixAchat", "PrixVente", "StockInitial", "StockAlerte", "TVA", "Unité", "Colisage"
                            };

                            var headers = new Dictionary<string, int>();
                            for (int col = 1; col <= worksheet.ColumnsUsed().Count(); col++)
                            {
                                string headerVal = worksheet.Cell(1, col).GetString()?.Trim();
                                if (!string.IsNullOrEmpty(headerVal))
                                {
                                    headers[headerVal] = col;
                                }
                            }

                            // Verify all columns exist
                            var missing = new List<string>();
                            foreach (var expected in expectedHeaders)
                            {
                                if (!headers.ContainsKey(expected))
                                {
                                    missing.Add(expected);
                                }
                            }

                            if (missing.Count > 0)
                            {
                                MessageBox.Show("Certaines colonnes obligatoires sont manquantes : " + string.Join(", ", missing), "Erreur de format Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            dgvArticles.Rows.Clear();

                            int rowNum = 2;
                            while (true)
                            {
                                var row = worksheet.Row(rowNum);
                                if (row.IsEmpty()) break;

                                string reference = row.Cell(headers["Référence"]).GetString()?.Trim();
                                string designation = row.Cell(headers["Désignation"]).GetString()?.Trim();
                                string barcode = row.Cell(headers["CodeBarre"]).GetString()?.Trim();

                                if (string.IsNullOrEmpty(reference) && string.IsNullOrEmpty(designation) && string.IsNullOrEmpty(barcode))
                                {
                                    // Check if the whole row is empty
                                    bool isEntireRowEmpty = true;
                                    foreach (var cell in row.CellsUsed())
                                    {
                                        if (!string.IsNullOrEmpty(cell.GetString()))
                                        {
                                            isEntireRowEmpty = false;
                                            break;
                                        }
                                    }
                                    if (isEntireRowEmpty) break;
                                }

                                string categoryName = row.Cell(headers["Catégorie"]).GetString()?.Trim();
                                string marqueName = row.Cell(headers["Marque"]).GetString()?.Trim();
                                string unitName = row.Cell(headers["Unité"]).GetString()?.Trim();
                                string tvaRaw = row.Cell(headers["TVA"]).GetString()?.Trim();

                                string tva = NormalizeTva(tvaRaw);

                                // Find matching IDs by name
                                int? categoryId = _categories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                                int? marqueId = _marques.FirstOrDefault(m => m.Name.Equals(marqueName, StringComparison.OrdinalIgnoreCase))?.Id;
                                int? unitId = _units.FirstOrDefault(u => u.Name.Equals(unitName, StringComparison.OrdinalIgnoreCase))?.Id;

                                string purchasePriceStr = row.Cell(headers["PrixAchat"]).GetString()?.Trim();
                                string salePriceStr = row.Cell(headers["PrixVente"]).GetString()?.Trim();
                                string stockInitialStr = row.Cell(headers["StockInitial"]).GetString()?.Trim();
                                string stockAlertStr = row.Cell(headers["StockAlerte"]).GetString()?.Trim();
                                string colisageStr = row.Cell(headers["Colisage"]).GetString()?.Trim();

                                // Add to DataGridView
                                int rowIndex = dgvArticles.Rows.Add(
                                    reference,
                                    designation,
                                    barcode,
                                    categoryId,
                                    marqueId,
                                    purchasePriceStr,
                                    salePriceStr,
                                    stockInitialStr,
                                    stockAlertStr,
                                    tva,
                                    unitId,
                                    colisageStr
                                );

                                rowNum++;
                            }

                            ValidateAllRows();
                            MessageBox.Show("Fichier Excel importé avec succès !", "Importation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'importation : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private decimal? ParseDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string normalized = text.Replace(",", ".");
            if (decimal.TryParse(normalized, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            return 0;
        }
        private string GenerateBarcode()
        {
            // Simple generation based on timestamp to ensure uniqueness
            return DateTime.Now.ToString("yyMMddHHmmss");
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private Button CreateStyledButton(string text, Color backColor, Size size, Point location, int fontSize)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = size,
                Location = location,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private CheckBox CreateStyledCheckBox(string text, Point location, bool isChecked)
        {
            return new CheckBox
            {
                Text = text,
                Font = StandardFont,
                Location = location,
                Size = new Size(200, 25),
                AutoSize = false,
                Checked = isChecked
            };
        }
    }
}
