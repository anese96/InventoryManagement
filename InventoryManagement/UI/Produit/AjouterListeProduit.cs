using InventoryManagement.Data;
using InventoryManagement.Data.Models;
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
    public partial class AjouterListeProduit : Form
    {
        private DataGridView dgvArticles;
        private CheckBox cbAutoRef;
        private CheckBox cbAutcode;

        private readonly Color PrimaryColor = Color.FromArgb(52, 152, 219);
        private readonly Color SuccessColor = Color.FromArgb(46, 204, 113);
        private readonly Font HeaderFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Font StandardFont = new Font("Segoe UI", 10, FontStyle.Bold);

        public AjouterListeProduit()
        {
            InitializeComponent();
            InitializeCustomComponents();
            BtnAddRow_Click(null, null);
        }

        private void InitializeCustomComponents()
        {


            List<Category> categories;
            List<Unit> units;
            List<Marque> marques;
            using (var db = new AppDbContext())
            {
                categories = db.Categories.OrderBy(c => c.Name).ToList();
                units = db.Units.OrderBy(u => u.Name).ToList();
                marques = db.Marques.OrderBy(m => m.Name).ToList();
            }

            this.Text = "Ajouter list Produits";
            this.Size = new Size(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
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

            yPosition += 40;

            // DataGridView Configuration
            dgvArticles = new DataGridView
            {
                Name = "dgvArticles",
                Location = new Point(10, yPosition),
                Size = new Size(mainPanel.Width - 40, mainPanel.Height - yPosition - 80),
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
            dgvArticles.Columns.Add("Reference", "Référence");
            dgvArticles.Columns.Add("Description", "Désignation");
            dgvArticles.Columns.Add("BarCode", "Code Barre");
            var categoryColumn = new DataGridViewComboBoxColumn
            {
                Name = "Category",
                HeaderText = "Catégorie",
                DataSource = categories,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat
            };
            dgvArticles.Columns.Add(categoryColumn);

            var marqueColumn = new DataGridViewComboBoxColumn
            {
                Name = "Marque",
                HeaderText = "Marque",
                DataSource = marques,
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
            //dgvArticles.cc
            var unitColumn = new DataGridViewComboBoxColumn
            {
                Name = "Unit",
                HeaderText = "Unité",
                DataSource = units,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat
            };
            dgvArticles.Columns.Add(unitColumn);
            dgvArticles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Packing", HeaderText = "Colisage", DefaultCellStyle = intStyle });

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
            btnSave.Click += BtnSave_Click;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    int savedCount = 0;
                    foreach (DataGridViewRow row in dgvArticles.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string designation = row.Cells["Description"].Value?.ToString();

                        // Basic validation
                        if (string.IsNullOrWhiteSpace(designation))
                        {
                            continue; // Skip invalid rows or show error? For now skipping empty designations
                        }
                        var product = new Product
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

                        db.Products.Add(product);
                        savedCount++;
                    }

                    if (savedCount > 0)
                    {
                        db.SaveChanges();
                        MessageBox.Show($"{savedCount} produit(s) enregistré(s) avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Aucun produit valide à enregistrer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
