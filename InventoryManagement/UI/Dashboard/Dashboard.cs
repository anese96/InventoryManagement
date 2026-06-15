using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InventoryManagement.UI.Dashboard
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Tableau de bord";
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Padding = new Padding(16);
            this.MinimumSize = new Size(1100, 700);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(mainLayout);

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.FromArgb(33, 37, 41),
                Padding = new Padding(24),
                Margin = new Padding(0, 0, 0, 20)
            };
            mainLayout.Controls.Add(headerPanel, 0, 0);

            var headerTitle = new Label
            {
                Text = "Tableau de bord",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            headerPanel.Controls.Add(headerTitle);

            var headerSubtitle = new Label
            {
                Text = "Vue d'ensemble des activités, des ventes et des performances",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(200, 210, 220),
                AutoSize = true,
                Location = new Point(0, 54)
            };
            headerPanel.Controls.Add(headerSubtitle);

            var kpiPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 0, 0, 18)
            };
            mainLayout.Controls.Add(kpiPanel, 0, 1);

            var kpis = new (string title, string icon, Color accentColor)[]
            {
             ("Chiffre d'affaires", "💰", Color.FromArgb(33, 150, 243)),
             ("Produits actifs", "📦", Color.FromArgb(76, 175, 80)),
             ("Stock total", "🏪", Color.FromArgb(255, 193, 7)),
             ("Rupture de stock", "⚠️", Color.FromArgb(244, 67, 54)),
             ("Ventes ce mois", "🛒", Color.FromArgb(0, 188, 212)),
             ("Achats ce mois", "🚚", Color.FromArgb(156, 39, 176)),
             ("Clients actifs", "👥", Color.FromArgb(63, 81, 181)),
             ("Fournisseurs actifs", "🏢", Color.FromArgb(255, 87, 34))
            };

            foreach (var k in kpis)
                kpiPanel.Controls.Add(CreateKpiCard(k.title, k.icon, k.accentColor));

            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0)
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            mainLayout.Controls.Add(contentLayout, 0, 2);

            var leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(0, 0, 12, 0)
            };
            contentLayout.Controls.Add(leftPanel, 0, 0);

            leftPanel.Controls.Add(CreateChartPlaceholder("Ventes sur 30 jours"));
            leftPanel.Controls.Add(CreateChartPlaceholder("Achats sur 30 jours"));
            leftPanel.Controls.Add(CreateChartPlaceholder("Bénéfice mensuel"));

            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false
            };
            contentLayout.Controls.Add(rightPanel, 1, 0);

            rightPanel.Controls.Add(CreateListPlaceholder("Top produits vendus"));
            rightPanel.Controls.Add(CreateListPlaceholder("Retours récents"));
            rightPanel.Controls.Add(CreateListPlaceholder("Pertes & casse"));
        }

        private Panel CreateKpiCard(string title, string icon, Color accentColor)
        {
            var card = new Panel
            {
                Width = 280,
                Height = 110,
                Margin = new Padding(8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var accent = new Panel
            {
                Width = 6,
                Dock = DockStyle.Left,
                BackColor = accentColor
            };

            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 26F, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(18, 24)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Gray,
                Location = new Point(78, 18),
                Size = new Size(180, 40),
                AutoSize = false,
                MaximumSize = new Size(180, 40)
            };

            var valueLabel = new Label
            {
                Text = "—",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(78, 58),
                AutoSize = true
            };

            var detailLabel = new Label
            {
                Text = "Actualisé maintenant",
                Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(145, 145, 145),
                Location = new Point(78, 82),
                AutoSize = true
            };

            card.Controls.Add(accent);
            card.Controls.Add(iconLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(detailLabel);

            return card;
        }

        private Panel CreateChartPlaceholder(string title)
        {
            var panel = new Panel
            {
                Height = 260,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(16, 16),
                AutoSize = true
            };

            var placeholder = new Label
            {
                Text = "Graphique en cours de chargement...",
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(16, 52),
                AutoSize = true
            };

            var infoLabel = new Label
            {
                Text = "Intégrez ici un graphique réel pour suivre vos tendances clés.",
                ForeColor = Color.FromArgb(135, 135, 135),
                Location = new Point(16, 78),
                AutoSize = true
            };

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(placeholder);
            panel.Controls.Add(infoLabel);
            return panel;
        }

        private Panel CreateListPlaceholder(string title)
        {
            var panel = new Panel
            {
                Height = 240,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(16, 16),
                AutoSize = true
            };

            var list = new ListView
            {
                Location = new Point(16, 52),
                Size = new Size(330, 160),
                View = View.Details,
                HeaderStyle = ColumnHeaderStyle.None,
                FullRowSelect = true,
                GridLines = false,
                BackColor = Color.FromArgb(247, 248, 250),
                BorderStyle = BorderStyle.None
            };
            list.Columns.Add("", 300);
            list.Items.Add(new ListViewItem("1. Article A"));
            list.Items.Add(new ListViewItem("2. Article B"));
            list.Items.Add(new ListViewItem("3. Article C"));

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(list);
            return panel;
        }
    }
}
