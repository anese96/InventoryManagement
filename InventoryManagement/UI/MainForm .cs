using InventoryManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public class MainForm : Form
    {
        private DataGridView grid;
        private List<Product> data = new List<Product>();

        public MainForm()
        {
            Width = 800;
            Height = 500;

            grid = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoGenerateColumns = true
            };

            var btnAdd = new Button { Text = "Add", Top = 320, Left = 50 };
            var btnEdit = new Button { Text = "Edit", Top = 320, Left = 150 };
            var btnDelete = new Button { Text = "Delete", Top = 320, Left = 250 };

            btnAdd.Click += (s, e) =>
            {
                var form = FormGenerator.GenerateForm<Product>();
                form.ShowDialog();
            };

            btnEdit.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Product p)
                {
                    var form = FormGenerator.GenerateForm(p);
                    form.ShowDialog();
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Product p)
                {
                    data.Remove(p);
                    RefreshGrid();
                }
            };

            Controls.Add(grid);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            grid.DataSource = null;
            grid.DataSource = data;
        }
    }
}
