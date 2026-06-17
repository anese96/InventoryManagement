using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace InventoryManagement.UI.Dashboard
{
    public partial class Dashboard : Form
    {
        // ════════════════════════════════════════════════════════
        //  PALETTE – Light Professional
        // ════════════════════════════════════════════════════════
        static readonly Color C_PageBg      = Color.FromArgb(245, 247, 252);
        static readonly Color C_SectionBg   = Color.FromArgb(235, 238, 248);
        static readonly Color C_CardBg      = Color.White;
        static readonly Color C_CardHover   = Color.FromArgb(235, 242, 255);
        static readonly Color C_Border      = Color.FromArgb(210, 218, 235);
        static readonly Color C_TextWhite   = Color.FromArgb(22,  32,  60);
        static readonly Color C_TextGray    = Color.FromArgb(70,  85, 115);
        static readonly Color C_TextMuted   = Color.FromArgb(140, 155, 180);

        static readonly Color A_Blue    = Color.FromArgb(59,  130, 246);
        static readonly Color A_Green   = Color.FromArgb(16,  185, 129);
        static readonly Color A_Purple  = Color.FromArgb(139,  92, 246);
        static readonly Color A_Orange  = Color.FromArgb(245, 158,  11);
        static readonly Color A_Red     = Color.FromArgb(239,  68,  68);
        static readonly Color A_Cyan    = Color.FromArgb(6,   182, 212);
        static readonly Color A_Pink    = Color.FromArgb(236,  72, 153);
        static readonly Color A_Indigo  = Color.FromArgb(99,  102, 241);

        // ════════════════════════════════════════════════════════
        //  MOCK DATA
        // ════════════════════════════════════════════════════════
        struct KpiDef
        {
            public string Icon, Title, Value, Trend;
            public Color  Accent;
            public bool   TrendUp;   // true = green, false = red/orange
        }

        readonly KpiDef[] _kpiFinance = {
            new KpiDef { Icon="💰", Title="Total Chiffre d'Affaires", Value="1 845 320 DA", Trend="+12.4 %", Accent=A_Blue,   TrendUp=true  },
            new KpiDef { Icon="📈", Title="Bénéfice du Mois",         Value="86 450 DA",   Trend="+8.1 %",  Accent=A_Green,  TrendUp=true  },
            new KpiDef { Icon="🛒", Title="Ventes du Mois",           Value="284 750 DA",  Trend="+15.6 %", Accent=A_Purple, TrendUp=true  },
            new KpiDef { Icon="🚚", Title="Achats du Mois",           Value="198 300 DA",  Trend="+3.2 %",  Accent=A_Orange, TrendUp=true  },
        };

        readonly KpiDef[] _kpiAujd = {
            new KpiDef { Icon="💵", Title="CA Aujourd'hui",       Value="12 380 DA", Trend="+5.2 %", Accent=A_Cyan,   TrendUp=true  },
            new KpiDef { Icon="🧾", Title="Ventes Aujourd'hui",   Value="47",        Trend="+2 vts", Accent=A_Blue,   TrendUp=true  },
            new KpiDef { Icon="📊", Title="Marge Bénéficiaire",   Value="30.4 %",    Trend="+1.3pt", Accent=A_Indigo, TrendUp=true  },
        };

        readonly KpiDef[] _kpiStock = {
            new KpiDef { Icon="📦", Title="Total Produits",        Value="2 847",         Trend="en catalogue",   Accent=A_Blue,   TrendUp=true  },
            new KpiDef { Icon="🏪", Title="Valeur Totale Stock",   Value="4 128 500 DA",  Trend="en inventaire",  Accent=A_Green,  TrendUp=true  },
            new KpiDef { Icon="⚠️", Title="Rupture de Stock",      Value="23",            Trend="→ urgent",       Accent=A_Red,    TrendUp=false },
            new KpiDef { Icon="🔶", Title="Faible Stock",           Value="57",            Trend="→ à réapprovisioner", Accent=A_Orange, TrendUp=false },
        };

        readonly KpiDef[] _kpiPartners = {
            new KpiDef { Icon="👥", Title="Clients Actifs",        Value="1 234",        Trend="comptes ouverts",  Accent=A_Cyan,   TrendUp=true  },
            new KpiDef { Icon="🏢", Title="Fournisseurs Actifs",   Value="89",           Trend="référencés",       Accent=A_Purple, TrendUp=true  },
            new KpiDef { Icon="💳", Title="Créances Clients",      Value="142 600 DA",   Trend="à recouvrir",      Accent=A_Pink,   TrendUp=false },
            new KpiDef { Icon="💸", Title="Dettes Fournisseurs",   Value="98 750 DA",    Trend="à régler",         Accent=A_Red,    TrendUp=false },
        };

        readonly KpiDef[] _kpiAlertes = {
            new KpiDef { Icon="🔄", Title="Retours Produits",  Value="18",        Trend="ce mois",    Accent=A_Orange, TrendUp=false },
            new KpiDef { Icon="📉", Title="Pertes & Casse",    Value="5 200 DA",  Trend="ce mois",    Accent=A_Red,    TrendUp=false },
            new KpiDef { Icon="📋", Title="Taux de Retours",   Value="6.3 %",     Trend="vs 4.1% N-1",Accent=A_Pink,   TrendUp=false },
        };

        readonly (string Produit, string Qte, string CA)[] _topProduits = {
            ("Huile Moteur 5W30 1L",          "1 240", "248 000 DA"),
            ("Filtre à Air Premium",           "980",   "196 000 DA"),
            ("Bougie d'Allumage NGK",          "870",   "87 000 DA"),
            ("Batterie 60Ah 12V",              "650",   "390 000 DA"),
            ("Plaquettes de Frein ATE",        "540",   "108 000 DA"),
            ("Courroie Distribution",          "430",   "86 000 DA"),
            ("Disque de Frein Brembo",         "390",   "117 000 DA"),
            ("Liquide de Refroidissement",     "370",   "55 500 DA"),
        };

        Color[] _rowAccents = { A_Blue, A_Green, A_Purple, A_Orange, A_Cyan, A_Pink, A_Indigo, A_Red };

        // ════════════════════════════════════════════════════════
        //  STATE
        // ════════════════════════════════════════════════════════
        Timer  _clock;
        Label  _lblClock;

        // ════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════
        public Dashboard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint  |
                     ControlStyles.UserPaint, true);
            Build();
        }

        // ════════════════════════════════════════════════════════
        //  MAIN BUILD
        // ════════════════════════════════════════════════════════
        void Build()
        {
            BackColor = C_PageBg;
            Dock      = DockStyle.Fill;

            // ── Outer scroll wrapper ──────────────────────────────
            var scroll = new Panel {
                Dock = DockStyle.Fill, AutoScroll = true, BackColor = C_PageBg
            };
            EnableDoubleBuffer(scroll);
            Controls.Add(scroll);

            // ── Content flow ──────────────────────────────────────
            var flow = new FlowLayoutPanel {
                FlowDirection    = FlowDirection.TopDown,
                WrapContents     = false,
                AutoSize         = true,
                AutoSizeMode     = AutoSizeMode.GrowAndShrink,
                BackColor        = C_PageBg,
                Padding          = new Padding(20, 16, 20, 30),
                Width            = scroll.Width > 0 ? scroll.Width : 1300,
            };
            EnableDoubleBuffer(flow);
            scroll.Controls.Add(flow);
            scroll.Resize += (s,e) => flow.Width = scroll.Width;

            // ── Sections ──────────────────────────────────────────
            AddHeader(flow);
            AddGap(flow, 14);

            AddSection(flow, "💰  Vue Financière Globale",    _kpiFinance,  4);
            AddSection(flow, "💵  Performances d'Aujourd'hui", _kpiAujd,    3);
            AddSection(flow, "📦  État du Stock & Produits",   _kpiStock,   4);
            AddSection(flow, "👥  Partenaires & Finances",     _kpiPartners,4);
            AddSection(flow, "🔄  Alertes & Anomalies",        _kpiAlertes, 3);
            AddTopProduits(flow);

            // ── Clock ─────────────────────────────────────────────
            _clock = new Timer { Interval = 1000 };
            _clock.Tick += (s,e) => { if (!_lblClock.IsDisposed) _lblClock.Text = DateTime.Now.ToString("HH:mm:ss"); };
            _clock.Start();
        }

        // ════════════════════════════════════════════════════════
        //  HEADER
        // ════════════════════════════════════════════════════════
        void AddHeader(FlowLayoutPanel flow)
        {
            var pnl = MakePanel(C_CardBg, new Size(flow.Width - 40, 76));
            pnl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            EnableDoubleBuffer(pnl);
            flow.Resize += (s,e) => pnl.Width = flow.Width - 40;

            pnl.Paint += (s,e) => {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // gradient bg : light blue → white
                using var br = new LinearGradientBrush(pnl.ClientRectangle,
                    Color.FromArgb(59, 130, 246), Color.FromArgb(99, 102, 241),
                    LinearGradientMode.Horizontal);
                using var path = RoundRect(pnl.ClientRectangle, 12);
                g.FillPath(br, path);
                // accent left stripe
                using var stripe = new SolidBrush(Color.White);
                using var sp = RoundRect(new Rectangle(0, 10, 5, pnl.Height-20), 3);
                g.FillPath(stripe, sp);
                // decorative circles
                using var deco = new SolidBrush(Color.FromArgb(25, 255, 255, 255));
                g.FillEllipse(deco, pnl.Width - 100, -30, 130, 130);
                g.FillEllipse(deco, pnl.Width - 220, 10, 80, 80);
            };

            // Title
            var title = MakeLabel(
                "   📊  TABLEAU DE BORD ADMINISTRATEUR",
                new Font("Segoe UI", 15f, FontStyle.Bold),
                Color.White, Color.Transparent,
                new Point(14, 10));
            pnl.Controls.Add(title);

            // Subtitle
            var sub = MakeLabel(
                $"   Gestion de Stock & Inventaire  ·  {DateTime.Now:dddd d MMMM yyyy}",
                new Font("Segoe UI", 8.5f), Color.FromArgb(220, 230, 255), Color.Transparent,
                new Point(16, 42));
            pnl.Controls.Add(sub);

            // Clock
            _lblClock = MakeLabel(
                DateTime.Now.ToString("HH:mm:ss"),
                new Font("Segoe UI", 18f, FontStyle.Bold),
                A_Cyan, Color.Transparent, new Point(0, 16));
            pnl.Controls.Add(_lblClock);

            void LayoutClock() => _lblClock.Location = new Point(pnl.Width - _lblClock.PreferredWidth - 24, 16);
            LayoutClock();
            pnl.Resize += (s,e) => LayoutClock();

            flow.Controls.Add(pnl);
        }

        // ════════════════════════════════════════════════════════
        //  SECTION  (title + KPI grid)
        // ════════════════════════════════════════════════════════
        void AddSection(FlowLayoutPanel flow, string title, KpiDef[] kpis, int cols)
        {
            AddGap(flow, 10);

            // Section title bar
            var titleBar = MakePanel(Color.Transparent, new Size(flow.Width - 40, 28));
            flow.Resize += (s,e) => titleBar.Width = flow.Width - 40;
            titleBar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // accent dot
                using var b = new SolidBrush(A_Blue);
                g.FillEllipse(b, 0, 6, 12, 12);
                // underline
                using var line = new SolidBrush(Color.FromArgb(60, A_Blue));
                g.FillRectangle(line, 0, titleBar.Height - 1, titleBar.Width, 1);
            };
            var lblTitle = MakeLabel(title,
                new Font("Segoe UI", 11f, FontStyle.Bold),
                C_TextWhite, Color.Transparent, new Point(20, 4));
            titleBar.Controls.Add(lblTitle);
            flow.Controls.Add(titleBar);

            AddGap(flow, 6);

            // KPI grid using TableLayoutPanel
            var grid = MakeKpiGrid(kpis, cols, flow.Width - 40);
            flow.Resize += (s,e) => RebuildGrid(grid, kpis, cols, flow.Width - 40);
            flow.Controls.Add(grid);
        }

        // ════════════════════════════════════════════════════════
        //  KPI GRID
        // ════════════════════════════════════════════════════════
        TableLayoutPanel MakeKpiGrid(KpiDef[] kpis, int cols, int totalW)
        {
            int rows    = (int)Math.Ceiling(kpis.Length / (double)cols);
            int gap     = 10;
            int cardH   = 110;
            int cardW   = Math.Max(1, (totalW - gap * (cols - 1)) / cols);

            var grid = new TableLayoutPanel
            {
                ColumnCount   = cols,
                RowCount      = rows,
                AutoSize      = true,
                AutoSizeMode  = AutoSizeMode.GrowAndShrink,
                BackColor     = Color.Transparent,
                Padding       = Padding.Empty,
                Margin        = Padding.Empty,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Width         = totalW,
            };
            EnableDoubleBuffer(grid);

            for (int c = 0; c < cols; c++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cardW + gap));
            for (int r = 0; r < rows; r++)
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, cardH + gap));

            for (int i = 0; i < kpis.Length; i++)
            {
                var card = BuildKpiCard(kpis[i], new Size(cardW, cardH));
                card.Margin = new Padding(0, 0, gap, gap);
                grid.Controls.Add(card, i % cols, i / cols);
            }

            return grid;
        }

        void RebuildGrid(TableLayoutPanel grid, KpiDef[] kpis, int cols, int totalW)
        {
            int gap   = 10;
            int cardW = Math.Max(1, (totalW - gap * (cols - 1)) / cols);

            grid.Width = totalW;
            for (int c = 0; c < grid.ColumnStyles.Count; c++)
            {
                grid.ColumnStyles[c].SizeType = SizeType.Absolute;
                grid.ColumnStyles[c].Width    = cardW + gap;
            }

            // Resize each card
            foreach (Control ctrl in grid.Controls)
                ctrl.Width = cardW;
        }

        // ════════════════════════════════════════════════════════
        //  KPI CARD
        // ════════════════════════════════════════════════════════
        Panel BuildKpiCard(KpiDef kpi, Size sz)
        {
            bool isHovered = false;

            var card = new Panel
            {
                Size      = sz,
                BackColor = C_CardBg,
                Cursor    = Cursors.Hand,
            };
            EnableDoubleBuffer(card);

            // — Custom paint (rounded bg + accent bar) ————————
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Background
                Color bg = isHovered ? C_CardHover : C_CardBg;
                using var bgPath = RoundRect(new Rectangle(0, 0, card.Width, card.Height), 12);
                using var bgBrush = new SolidBrush(bg);
                g.FillPath(bgBrush, bgPath);

                // Border
                using var borderPen = new Pen(C_Border, 1.5f);
                g.DrawPath(borderPen, bgPath);

                // Left accent stripe
                using var stripePath = RoundRect(new Rectangle(0, 12, 5, card.Height - 24), 3);
                using var stripeBrush = new SolidBrush(kpi.Accent);
                g.FillPath(stripeBrush, stripePath);

                // Soft accent tint top-right
                using var glowBrush = new SolidBrush(Color.FromArgb(15, kpi.Accent));
                g.FillEllipse(glowBrush, card.Width - 65, -25, 90, 90);
            };

            // ── Icon (top-right) ──────────────────────────────
            var lblIcon = new Label
            {
                Text      = kpi.Icon,
                Font      = new Font("Segoe UI Emoji", 20f),
                ForeColor = kpi.Accent,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(card.Width - 50, 8),
            };
            card.Controls.Add(lblIcon);
            card.Resize += (s, e) => lblIcon.Location = new Point(card.Width - 50, 8);

            // ── Title ─────────────────────────────────────────
            var lblTitle = new Label
            {
                Text      = kpi.Title,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = C_TextGray,
                BackColor = Color.Transparent,
                AutoSize  = false,
                Size      = new Size(card.Width - 58, 18),
                Location  = new Point(12, 10),
                TextAlign = ContentAlignment.MiddleLeft,
            };
            card.Controls.Add(lblTitle);
            card.Resize += (s, e) => lblTitle.Width = card.Width - 58;

            // ── Value ─────────────────────────────────────────
            float fs   = kpi.Value.Length > 13 ? 12.5f : kpi.Value.Length > 9 ? 14.5f : 19f;
            var lblVal = new Label
            {
                Text      = kpi.Value,
                Font      = new Font("Segoe UI", fs, FontStyle.Bold),
                ForeColor = C_TextWhite,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(12, 34),
            };
            card.Controls.Add(lblVal);

            // ── Trend badge ───────────────────────────────────
            Color trendColor = kpi.TrendUp ? A_Green : A_Orange;
            string trendIcon = kpi.TrendUp ? "▲ " : "▼ ";

            var lblTrend = new Label
            {
                Text      = trendIcon + kpi.Trend,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = trendColor,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(12, card.Height - 24),
            };
            card.Controls.Add(lblTrend);
            card.Resize += (s, e) => lblTrend.Location = new Point(12, card.Height - 24);

            // ── Hover ─────────────────────────────────────────
            Action hoverOn  = () => { isHovered = true;  card.Invalidate(); };
            Action hoverOff = () => { isHovered = false; card.Invalidate(); };

            card.MouseEnter += (s, e) => hoverOn();
            card.MouseLeave += (s, e) => hoverOff();
            foreach (Control c in card.Controls)
            {
                c.MouseEnter += (s, e) => hoverOn();
                c.MouseLeave += (s, e) => hoverOff();
            }

            return card;
        }

        // ════════════════════════════════════════════════════════
        //  TOP PRODUITS TABLE
        // ════════════════════════════════════════════════════════
        void AddTopProduits(FlowLayoutPanel flow)
        {
            AddGap(flow, 10);

            // Section title
            var titleBar = MakePanel(Color.Transparent, new Size(flow.Width - 40, 28));
            flow.Resize += (s,e) => titleBar.Width = flow.Width - 40;
            titleBar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(A_Orange);
                g.FillEllipse(b, 0, 6, 12, 12);
                using var line = new SolidBrush(Color.FromArgb(60, A_Orange));
                g.FillRectangle(line, 0, titleBar.Height - 1, titleBar.Width, 1);
            };
            var lblT = MakeLabel("🎯  Produits les Plus Vendus",
                new Font("Segoe UI", 11f, FontStyle.Bold),
                C_TextWhite, Color.Transparent, new Point(20, 4));
            titleBar.Controls.Add(lblT);
            flow.Controls.Add(titleBar);
            AddGap(flow, 6);

            // Table container
            int rowH   = 46;
            int hdrH   = 44;
            int tableH = hdrH + _topProduits.Length * rowH + 4;
            int tableW = flow.Width - 40;

            var table = MakePanel(C_CardBg, new Size(tableW, tableH));
            EnableDoubleBuffer(table);
            flow.Resize += (s,e) => { table.Width = flow.Width - 40; RedrawTable(table); };
            table.Paint += (s,e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundRect(table.ClientRectangle, 12);
                using var brush = new SolidBrush(C_CardBg);
                g.FillPath(brush, path);
                using var pen = new Pen(C_Border, 1);
                g.DrawPath(pen, path);
            };

            // Header row
            var hdr = MakePanel(Color.FromArgb(240, 244, 252), new Size(tableW, hdrH));
            EnableDoubleBuffer(hdr);
            hdr.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundRect(new Rectangle(0, 0, hdr.Width, hdr.Height*2), 12);
                using var brush = new SolidBrush(Color.FromArgb(240, 244, 252));
                g.FillPath(brush, path);
                using var sepPen = new Pen(C_Border, 1);
                g.DrawLine(sepPen, 0, hdr.Height-1, hdr.Width, hdr.Height-1);
            };
            table.Controls.Add(hdr);
            table.Resize += (s,e) => hdr.Width = table.Width;

            AddHeaderLabel(hdr, "#",                     A_Blue,   ContentAlignment.MiddleCenter, () => new Rectangle(0,     0, 48,     hdrH));
            AddHeaderLabel(hdr, "Produit",               C_TextGray, ContentAlignment.MiddleLeft, () => new Rectangle(52,    0, hdr.Width/2 - 52, hdrH));
            AddHeaderLabel(hdr, "Quantité Vendue",       C_TextGray, ContentAlignment.MiddleCenter,() => new Rectangle(hdr.Width/2 + 10, 0, hdr.Width/4 - 10, hdrH));
            AddHeaderLabel(hdr, "Chiffre d'Affaires",    C_TextGray, ContentAlignment.MiddleCenter,() => new Rectangle(3*hdr.Width/4,    0, hdr.Width/4 - 10, hdrH));

            // Data rows
            for (int i = 0; i < _topProduits.Length; i++)
            {
                var p       = _topProduits[i];
                Color accent= _rowAccents[i % _rowAccents.Length];
                Color rowBg = i % 2 == 0 ? C_CardBg : Color.FromArgb(248, 250, 255);
                bool odd    = i % 2 != 0;

                var row = MakePanel(rowBg, new Size(tableW, rowH));
                row.Location = new Point(0, hdrH + i * rowH);
                EnableDoubleBuffer(row);
                table.Controls.Add(row);
                table.Resize += (s,e) => row.Width = table.Width;

                row.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    // colored left dot
                    using var dotBrush = new SolidBrush(accent);
                    g.FillEllipse(dotBrush, 56, rowH/2 - 5, 10, 10);
                    // separator
                    using var sepPen = new Pen(C_Border, 1);
                    g.DrawLine(sepPen, 0, rowH-1, row.Width, rowH-1);
                };

                // Rank
                var lblRank = new Label {
                    Text = $"#{i+1}", Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = accent, BackColor = Color.Transparent,
                    AutoSize = false, Size = new Size(48, rowH),
                    Location = new Point(0, 0), TextAlign = ContentAlignment.MiddleCenter,
                };
                row.Controls.Add(lblRank);

                // Product name
                var lblName = new Label {
                    Text = p.Produit, Font = new Font("Segoe UI", 9.5f),
                    ForeColor = C_TextWhite, BackColor = Color.Transparent,
                    AutoSize = false, Size = new Size(0, rowH),
                    Location = new Point(70, 0), TextAlign = ContentAlignment.MiddleLeft,
                };
                row.Controls.Add(lblName);
                row.Resize += (s,e) => lblName.Width = row.Width/2 - 70;

                // Qty
                var lblQty = new Label {
                    Text = p.Qte, Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = A_Cyan, BackColor = Color.Transparent,
                    AutoSize = false, Size = new Size(0, rowH),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                row.Controls.Add(lblQty);
                row.Resize += (s,e) => { lblQty.Location = new Point(row.Width/2+10, 0); lblQty.Width = row.Width/4-10; };

                // CA
                var lblCA = new Label {
                    Text = p.CA, Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = A_Green, BackColor = Color.Transparent,
                    AutoSize = false, Size = new Size(0, rowH),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                row.Controls.Add(lblCA);
                row.Resize += (s,e) => { lblCA.Location = new Point(3*row.Width/4, 0); lblCA.Width = row.Width/4-10; };

                // hover
                Action on  = () => { row.BackColor = C_CardHover; };
                Action off = () => { row.BackColor = rowBg; };
                row.MouseEnter += (s,e) => on(); row.MouseLeave += (s,e) => off();
                foreach (Control c in row.Controls)
                { c.MouseEnter += (s,e) => on(); c.MouseLeave += (s,e) => off(); }

                // Trigger resize to position columns
                row.Width = tableW;
            }

            flow.Controls.Add(table);
            AddGap(flow, 20);
        }

        void RedrawTable(Panel table) { table.Invalidate(); foreach (Control c in table.Controls) c.Invalidate(); }

        void AddHeaderLabel(Panel hdr, string text, Color color,
            ContentAlignment align, Func<Rectangle> bounds)
        {
            var lbl = new Label {
                Text = text, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = color, BackColor = Color.Transparent,
                AutoSize = false, TextAlign = align,
            };
            void Layout() { var b = bounds(); lbl.Bounds = b; }
            Layout();
            hdr.Controls.Add(lbl);
            hdr.Resize += (s,e) => Layout();
        }

        // ════════════════════════════════════════════════════════
        //  HELPERS
        // ════════════════════════════════════════════════════════
        static void AddGap(FlowLayoutPanel flow, int h)
        {
            flow.Controls.Add(new Panel { Width = 1, Height = h, BackColor = Color.Transparent });
        }

        static Panel MakePanel(Color bg, Size size) =>
            new Panel { BackColor = bg, Size = size, Margin = Padding.Empty };

        static Label MakeLabel(string text, Font font, Color fore, Color back, Point loc) =>
            new Label { Text = text, Font = font, ForeColor = fore, BackColor = back,
                        AutoSize = true, Location = loc };

        static void EnableDoubleBuffer(Control c)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance)
                ?.SetValue(c, true);
        }

        static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = Math.Max(1, Math.Min(radius * 2, Math.Min(r.Width, r.Height)));
            var path = new GraphicsPath();
            path.AddArc(r.X,          r.Y,           d, d, 180, 90);
            path.AddArc(r.Right-d,    r.Y,           d, d, 270, 90);
            path.AddArc(r.Right-d,    r.Bottom-d,    d, d, 0,   90);
            path.AddArc(r.X,          r.Bottom-d,    d, d, 90,  90);
            path.CloseFigure();
            return path;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clock?.Stop(); _clock?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
