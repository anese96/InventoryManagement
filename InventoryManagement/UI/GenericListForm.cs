using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace InventoryManagement.UI
{
    /// <summary>
    /// A fully generic list form that shows a <see cref="DataGridView"/> populated from a
    /// <see cref="IList{T}"/> and provides Add / Edit / Delete toolbar buttons.
    /// Wire up <see cref="OnAdd"/>, <see cref="OnEdit"/>, <see cref="OnDelete"/> to persist
    /// changes through your service layer.
    /// </summary>
    public class GenericListForm<T> : Form where T : new()
    {
        // ── Public events ────────────────────────────────────────────────────
        /// <summary>Raised when the user clicks Add and a new item was confirmed.</summary>
        public event Action<T> OnAdd;
        /// <summary>Raised when the user edits an existing row and confirms changes.</summary>
        public event Action<T> OnEdit;
        /// <summary>Raised when the user requests deletion of a row. Return false to abort.</summary>
        public event Func<T, bool> OnDelete;

        // ── State ────────────────────────────────────────────────────────────
        private IList<T>          _data;
        private readonly ICrudFormContext _context;
        private readonly string   _entityTitle;

        // ── Controls ─────────────────────────────────────────────────────────
        private DataGridView dgv;
        private TextBox      txtSearch;
        private Label        lblCount;

        // ──────────────────────────────────────────────────────────────────────

        /// <param name="data">Initial data list to display.</param>
        /// <param name="context">Optional lookup context for ComboBox fields in add/edit forms.</param>
        /// <param name="title">Window title override.</param>
        public GenericListForm(IList<T> data, ICrudFormContext context = null, string title = null)
        {
            _data        = data ?? new List<T>();
            _context     = context;
            _entityTitle = title ?? CrudFormGenerator_FriendlyName();

            InitShell();
            BuildGrid();
            RefreshGrid();
        }

        // ── Shell ────────────────────────────────────────────────────────────

        private void InitShell()
        {
            Text            = _entityTitle;
            Size            = new Size(1000, 620);
            StartPosition   = FormStartPosition.CenterParent;
            MinimumSize     = new Size(600, 800);
            BackColor       = Color.White;

            var mainLayout = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = 1,
                RowCount    = 4
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));   // header
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));   // toolbar
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // grid
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));   // status bar
            Controls.Add(mainLayout);

            // ── Header ───────────────────────────────────────────────────────
            var header = new Label
            {
                Text      = "  " + _entityTitle,
                Font      = CrudTheme.FontHeader,
                ForeColor = CrudTheme.HeaderFg,
                BackColor = CrudTheme.HeaderBg,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainLayout.Controls.Add(header, 0, 0);

            // ── Toolbar ───────────────────────────────────────────────────────
            var toolbar = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 250),
                Padding   = new Padding(8, 8, 8, 0)
            };
            mainLayout.Controls.Add(toolbar, 0, 1);

            var btnAdd = MakeToolbarBtn("➕  Ajouter", CrudTheme.ButtonSave);
            btnAdd.Click += BtnAdd_Click;

            var btnEdit = MakeToolbarBtn("✏️  Modifier", Color.FromArgb(52, 152, 219));
            btnEdit.Click += BtnEdit_Click;

            var btnDelete = MakeToolbarBtn("🗑️  Supprimer", CrudTheme.ButtonCancel);
            btnDelete.Click += BtnDelete_Click;

            var btnRefresh = MakeToolbarBtn("🔄  Actualiser", Color.FromArgb(127, 140, 141));
            btnRefresh.Click += (s, e) => RefreshGrid();

            // Search box
            txtSearch = new TextBox
            {
                Width       = 220,
                Height      = 32,
                Font        = CrudTheme.FontInput,
                PlaceholderText = "🔍  Rechercher…"
            };
            txtSearch.TextChanged += (s, e) => FilterGrid(txtSearch.Text);

            var searchLabel = new Label
            {
                Text      = "Recherche:",
                Font      = CrudTheme.FontLabel,
                AutoSize  = true,
                ForeColor = CrudTheme.LabelFg
            };

            // Layout toolbar items left-to-right
            int x = 8;
            foreach (var btn in new[] { btnAdd, btnEdit, btnDelete, btnRefresh })
            {
                btn.Location = new Point(x, 4);
                toolbar.Controls.Add(btn);
                x += btn.Width + 8;
            }
            x += 20;
            searchLabel.Location = new Point(x, 10);
            toolbar.Controls.Add(searchLabel);
            txtSearch.Location = new Point(x + searchLabel.PreferredWidth + 6, 4);
            toolbar.Controls.Add(txtSearch);

            // ── Grid container ────────────────────────────────────────────────
            var gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 4, 8, 4) };
            mainLayout.Controls.Add(gridPanel, 0, 2);

            dgv = new DataGridView
            {
                Dock                        = DockStyle.Fill,
                BackgroundColor             = Color.White,
                BorderStyle                 = BorderStyle.None,
                AllowUserToAddRows          = false,
                AllowUserToDeleteRows       = false,
                ReadOnly                    = true,
                SelectionMode               = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect                 = false,
                RowHeadersVisible           = false,
                AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight         = 38,
                RowTemplate                 = { Height = 34 },
                EnableHeadersVisualStyles   = false,
                CellBorderStyle             = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor                   = Color.FromArgb(220, 221, 225)
            };

            // Header style
            dgv.ColumnHeadersDefaultCellStyle.BackColor  = CrudTheme.HeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = CrudTheme.HeaderBg;

            // Row alternating colours
            dgv.DefaultCellStyle.Font           = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);

            dgv.DoubleClick += (s, e) => BtnEdit_Click(s, e);

            gridPanel.Controls.Add(dgv);

            // ── Status bar ────────────────────────────────────────────────────
            var statusBar = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 241),
                Padding   = new Padding(10, 6, 0, 0)
            };
            mainLayout.Controls.Add(statusBar, 0, 3);

            lblCount = new Label
            {
                AutoSize  = true,
                ForeColor = Color.FromArgb(44, 62, 80),
                Font      = new Font("Segoe UI", 8)
            };
            statusBar.Controls.Add(lblCount);
        }

        // ── Grid ─────────────────────────────────────────────────────────────

        private List<PropertyInfo> _visibleProps;

        private void BuildGrid()
        {
            _visibleProps = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.GetCustomAttribute<FormSkipAttribute>() == null
                    && (p.PropertyType.IsPrimitive
                        || p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(decimal)
                        || p.PropertyType == typeof(DateTime)
                        || p.PropertyType == typeof(decimal?)
                        || p.PropertyType == typeof(int?)
                        || p.PropertyType == typeof(bool?)
                        || Nullable.GetUnderlyingType(p.PropertyType) != null))
                .ToList();

            dgv.Columns.Clear();
            foreach (var prop in _visibleProps)
            {
                var labelAttr = prop.GetCustomAttribute<FormLabelAttribute>();
                string header = labelAttr?.Label ?? SplitCamelCase(prop.Name);
                dgv.Columns.Add(prop.Name, header);
            }
        }

        private void RefreshGrid(IList<T> source = null)
        {
            source ??= _data;
            dgv.Rows.Clear();

            foreach (var item in source)
            {
                var values = _visibleProps.Select(p => p.GetValue(item)?.ToString() ?? "").ToArray();
                dgv.Rows.Add(values);
                dgv.Rows[dgv.Rows.Count - 1].Tag = item;
            }

            lblCount.Text = $"{dgv.RowCount} enregistrement(s)";
        }

        private void FilterGrid(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                RefreshGrid();
                return;
            }

            var filtered = _data.Where(item =>
                _visibleProps.Any(p =>
                {
                    var val = p.GetValue(item)?.ToString() ?? "";
                    return val.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                })).ToList();

            RefreshGrid(filtered);
        }

        // ── Toolbar handlers ─────────────────────────────────────────────────

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var res = CrudFormGenerator.ShowAddForm<T>(context: _context);
            if (!res.Saved) return;

            _data.Add(res.Instance);
            OnAdd?.Invoke(res.Instance);
            RefreshGrid();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null) { ShowNoSelectionWarning(); return; }

            var res = CrudFormGenerator.ShowEditForm<T>(item, context: _context);
            if (!res.Saved) return;

            // Replace in list (for value types / structs)
            var idx = _data.IndexOf(item);
            if (idx >= 0) _data[idx] = res.Instance;

            OnEdit?.Invoke(res.Instance);
            RefreshGrid();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null) { ShowNoSelectionWarning(); return; }

            var confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet enregistrement ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            bool canDelete = OnDelete?.Invoke(item) ?? true;
            if (!canDelete) return;

            _data.Remove(item);
            RefreshGrid();
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private T GetSelectedItem()
        {
            if (dgv.SelectedRows.Count == 0) return default;
            return (T)dgv.SelectedRows[0].Tag;
        }

        private static void ShowNoSelectionWarning()
            => MessageBox.Show("Veuillez sélectionner un enregistrement.",
                "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static Button MakeToolbarBtn(string text, Color back)
        {
            var btn = new Button
            {
                Text      = text,
                AutoSize  = false,
                Width     = 140,
                Height    = 36,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static string SplitCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var result = System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1").Trim();
            return char.ToUpper(result[0]) + result.Substring(1);
        }

        private static string CrudFormGenerator_FriendlyName()
        {
            var name = typeof(T).Name.Replace("Dto", "").Replace("DTO", "");
            return "Liste — " + SplitCamelCase(name);
        }
    }
}
