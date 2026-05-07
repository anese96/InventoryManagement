using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InventoryManagement.UI
{
    public class DataGridHelper
    {
        public DataGridView Grid { get; private set; }
        private List<object> allData;
        private BindingSource bs = new BindingSource();
        private TextBox txtSearch;

        public DataGridHelper(Control parentControl)
        {
            Panel searchPanel = new Panel
            {
                Location = new Point(30, 190),
                Size = new Size(400, 40),
                BackColor = Color.White
            };
            txtSearch = new TextBox
            {
                Size = new Size(350, 30),
                Location = new Point(45, 8),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.None,
                Text = "Rechercher...", 
                ForeColor = Color.Gray 
            };

            txtSearch.Enter += Search_Enter;
            txtSearch.Leave += Search_Leave;
            txtSearch.TextChanged += Search_TextChanged;
            
            Label searchIcon = new Label { Text = "🔍", Location = new Point(10, 8), AutoSize = true };
            Panel line = new Panel { BackColor = Color.FromArgb(52, 152, 219), Height = 2, Dock = DockStyle.Bottom };

            searchPanel.Controls.Add(searchIcon);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(line);
            parentControl.Controls.Add(searchPanel);

            Grid = new DataGridView
            {
                Location = new Point(30, 250),
                Size = new Size(1500, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            
            parentControl.Controls.Add(Grid);

            Grid.DataSource = bs;

            ApplyBaseDesign();
           // AttachDynamicEvents();
        }

        private void Search_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Rechercher...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void Search_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Rechercher...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Rechercher...")
                Filter("");
            else
                Filter(txtSearch.Text);
        }

        // 🎨 DESIGN BASE
        private void ApplyBaseDesign()
        {
            Grid.EnableHeadersVisualStyles = false;
            Grid.BorderStyle = BorderStyle.None;
            Grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            Grid.RowHeadersVisible = false;

            Grid.BackgroundColor = Color.White;

            Grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            Grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            Grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            Grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            Grid.DefaultCellStyle.SelectionForeColor = Color.White;

            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Grid.RowTemplate.Height = 40;
            Grid.ColumnHeadersHeight = 40;
            Grid.RowsDefaultCellStyle.BackColor = Color.White;
            Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            //Grid.CellDoubleClick += Grid_CellDoubleClick;
        }

        //public virtual void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        //{
            
        //}

        // ⚡ EVENTS DYNAMIQUES
        private void AttachDynamicEvents()
        {
            Grid.CellFormatting += Grid_CellFormatting;
        }

        // 🔥 DESIGN DYNAMIQUE (LOGIQUE MÉTIER)
        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Grid.Columns[e.ColumnIndex].Name == "Prix" && e.Value != null)
            {
                decimal prix = Convert.ToDecimal(e.Value);

                if (prix > 1000)
                {
                    e.CellStyle.BackColor = Color.MistyRose;
                    e.CellStyle.ForeColor = Color.DarkRed;
                }
                else if (prix > 500)
                {
                    e.CellStyle.BackColor = Color.LemonChiffon;
                    e.CellStyle.ForeColor = Color.DarkGoldenrod;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Honeydew;
                    e.CellStyle.ForeColor = Color.DarkGreen;
                }
            }
        }

        // 📦 DATA
        public void SetData<T>(List<T> data)
        {
            if (data == null) return;

            allData = data.Cast<object>().ToList();
            bs.DataSource = data;
        }

        // 🔎 FILTER
        public void Filter(string search)
        {
            if (allData == null) return;

            if (string.IsNullOrWhiteSpace(search))
            {
                bs.DataSource = allData;
                return;
            }


            search = search.ToLower();

            var filtered = allData.Where(item =>
                item.GetType().GetProperties().Any(prop =>
                {
                    var value = prop.GetValue(item);
                    return value != null &&
                           value.ToString().ToLower().Contains(search);
                })
            ).ToList();

            bs.DataSource = filtered;
        }
    }
}
