
namespace InventoryManagement.UI
{
    partial class MainDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sidebarPanel = new Panel();
            btnUsers = new Button();
            btnCommonRepositories = new Button();
            btnDataSpreadsheets = new Button();
            btnCashFlow = new Button();
            btnCounterSales = new Button();
            btnSalesReturns = new Button();
            btnPurchaseReturns = new Button();
            btnSuppliers = new Button();
            btnPurchases = new Button();
            btnSales = new Button();
            btnClients = new Button();
            btnProducts = new Button();
            btnDashboard = new Button();
            logoPanel = new Panel();
            pictureBox1 = new PictureBox();
            lblAppTitle = new Label();
            mainContentPanel = new Panel();
            sidebarPanel.SuspendLayout();
            logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.AutoScroll = true;
            sidebarPanel.BackColor = Color.FromArgb(52, 73, 94);
            sidebarPanel.Controls.Add(btnUsers);
            sidebarPanel.Controls.Add(btnCommonRepositories);
            sidebarPanel.Controls.Add(btnDataSpreadsheets);
            sidebarPanel.Controls.Add(btnCashFlow);
            sidebarPanel.Controls.Add(btnCounterSales);
            sidebarPanel.Controls.Add(btnSalesReturns);
            sidebarPanel.Controls.Add(btnPurchaseReturns);
            sidebarPanel.Controls.Add(btnSuppliers);
            sidebarPanel.Controls.Add(btnPurchases);
            sidebarPanel.Controls.Add(btnSales);
            sidebarPanel.Controls.Add(btnClients);
            sidebarPanel.Controls.Add(btnProducts);
            sidebarPanel.Controls.Add(btnDashboard);
            sidebarPanel.Controls.Add(logoPanel);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(250, 852);
            sidebarPanel.TabIndex = 0;
            // 
            // btnUsers
            // 
            btnUsers.Cursor = Cursors.Hand;
            btnUsers.Dock = DockStyle.Top;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUsers.ForeColor = Color.White;
            btnUsers.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsers.Location = new Point(0, 840);
            btnUsers.Name = "btnUsers";
            btnUsers.Padding = new Padding(20, 0, 0, 0);
            btnUsers.Size = new Size(229, 60);
            btnUsers.TabIndex = 13;
            btnUsers.Text = "   👥 Utilisateurs";
            btnUsers.TextAlign = ContentAlignment.MiddleLeft;
            btnUsers.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += BtnUsers_Click;
            // 
            // btnCommonRepositories
            // 
            btnCommonRepositories.Cursor = Cursors.Hand;
            btnCommonRepositories.Dock = DockStyle.Top;
            btnCommonRepositories.FlatAppearance.BorderSize = 0;
            btnCommonRepositories.FlatStyle = FlatStyle.Flat;
            btnCommonRepositories.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCommonRepositories.ForeColor = Color.White;
            btnCommonRepositories.ImageAlign = ContentAlignment.MiddleLeft;
            btnCommonRepositories.Location = new Point(0, 780);
            btnCommonRepositories.Name = "btnCommonRepositories";
            btnCommonRepositories.Padding = new Padding(20, 0, 0, 0);
            btnCommonRepositories.Size = new Size(229, 60);
            btnCommonRepositories.TabIndex = 12;
            btnCommonRepositories.Text = "   🗂️ Référentiels";
            btnCommonRepositories.TextAlign = ContentAlignment.MiddleLeft;
            btnCommonRepositories.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCommonRepositories.UseVisualStyleBackColor = true;
            btnCommonRepositories.Click += BtnCommonRepositories_Click;
            // 
            // btnDataSpreadsheets
            // 
            btnDataSpreadsheets.Cursor = Cursors.Hand;
            btnDataSpreadsheets.Dock = DockStyle.Top;
            btnDataSpreadsheets.FlatAppearance.BorderSize = 0;
            btnDataSpreadsheets.FlatStyle = FlatStyle.Flat;
            btnDataSpreadsheets.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDataSpreadsheets.ForeColor = Color.White;
            btnDataSpreadsheets.ImageAlign = ContentAlignment.MiddleLeft;
            btnDataSpreadsheets.Location = new Point(0, 720);
            btnDataSpreadsheets.Name = "btnDataSpreadsheets";
            btnDataSpreadsheets.Padding = new Padding(20, 0, 0, 0);
            btnDataSpreadsheets.Size = new Size(229, 60);
            btnDataSpreadsheets.TabIndex = 11;
            btnDataSpreadsheets.Text = "   📑 Tableurs Données";
            btnDataSpreadsheets.TextAlign = ContentAlignment.MiddleLeft;
            btnDataSpreadsheets.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDataSpreadsheets.UseVisualStyleBackColor = true;
            btnDataSpreadsheets.Click += BtnDataSpreadsheets_Click;
            // 
            // btnCashFlow
            // 
            btnCashFlow.Cursor = Cursors.Hand;
            btnCashFlow.Dock = DockStyle.Top;
            btnCashFlow.FlatAppearance.BorderSize = 0;
            btnCashFlow.FlatStyle = FlatStyle.Flat;
            btnCashFlow.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCashFlow.ForeColor = Color.White;
            btnCashFlow.ImageAlign = ContentAlignment.MiddleLeft;
            btnCashFlow.Location = new Point(0, 660);
            btnCashFlow.Name = "btnCashFlow";
            btnCashFlow.Padding = new Padding(20, 0, 0, 0);
            btnCashFlow.Size = new Size(229, 60);
            btnCashFlow.TabIndex = 10;
            btnCashFlow.Text = "   💵 Flux Financiers";
            btnCashFlow.TextAlign = ContentAlignment.MiddleLeft;
            btnCashFlow.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCashFlow.UseVisualStyleBackColor = true;
            btnCashFlow.Click += BtnCashFlow_Click;
            // 
            // btnCounterSales
            // 
            btnCounterSales.Cursor = Cursors.Hand;
            btnCounterSales.Dock = DockStyle.Top;
            btnCounterSales.FlatAppearance.BorderSize = 0;
            btnCounterSales.FlatStyle = FlatStyle.Flat;
            btnCounterSales.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCounterSales.ForeColor = Color.White;
            btnCounterSales.ImageAlign = ContentAlignment.MiddleLeft;
            btnCounterSales.Location = new Point(0, 600);
            btnCounterSales.Name = "btnCounterSales";
            btnCounterSales.Padding = new Padding(20, 0, 0, 0);
            btnCounterSales.Size = new Size(229, 60);
            btnCounterSales.TabIndex = 9;
            btnCounterSales.Text = "   🏪 Ventes Comptoir";
            btnCounterSales.TextAlign = ContentAlignment.MiddleLeft;
            btnCounterSales.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCounterSales.UseVisualStyleBackColor = true;
            // 
            // btnSalesReturns
            // 
            btnSalesReturns.Cursor = Cursors.Hand;
            btnSalesReturns.Dock = DockStyle.Top;
            btnSalesReturns.FlatAppearance.BorderSize = 0;
            btnSalesReturns.FlatStyle = FlatStyle.Flat;
            btnSalesReturns.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSalesReturns.ForeColor = Color.White;
            btnSalesReturns.ImageAlign = ContentAlignment.MiddleLeft;
            btnSalesReturns.Location = new Point(0, 540);
            btnSalesReturns.Name = "btnSalesReturns";
            btnSalesReturns.Padding = new Padding(20, 0, 0, 0);
            btnSalesReturns.Size = new Size(229, 60);
            btnSalesReturns.TabIndex = 8;
            btnSalesReturns.Text = "   ↩️ Retour Ventes";
            btnSalesReturns.TextAlign = ContentAlignment.MiddleLeft;
            btnSalesReturns.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSalesReturns.UseVisualStyleBackColor = true;
            btnSalesReturns.Click += BtnSalesReturns_Click;
            // 
            // btnPurchaseReturns
            // 
            btnPurchaseReturns.Cursor = Cursors.Hand;
            btnPurchaseReturns.Dock = DockStyle.Top;
            btnPurchaseReturns.FlatAppearance.BorderSize = 0;
            btnPurchaseReturns.FlatStyle = FlatStyle.Flat;
            btnPurchaseReturns.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPurchaseReturns.ForeColor = Color.White;
            btnPurchaseReturns.ImageAlign = ContentAlignment.MiddleLeft;
            btnPurchaseReturns.Location = new Point(0, 480);
            btnPurchaseReturns.Name = "btnPurchaseReturns";
            btnPurchaseReturns.Padding = new Padding(20, 0, 0, 0);
            btnPurchaseReturns.Size = new Size(229, 60);
            btnPurchaseReturns.TabIndex = 7;
            btnPurchaseReturns.Text = "   ↩️ Retour Achats";
            btnPurchaseReturns.TextAlign = ContentAlignment.MiddleLeft;
            btnPurchaseReturns.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnPurchaseReturns.UseVisualStyleBackColor = true;
            btnPurchaseReturns.Click += BtnPurchaseReturns_Click;
            // 
            // btnSuppliers
            // 
            btnSuppliers.Cursor = Cursors.Hand;
            btnSuppliers.Dock = DockStyle.Top;
            btnSuppliers.FlatAppearance.BorderSize = 0;
            btnSuppliers.FlatStyle = FlatStyle.Flat;
            btnSuppliers.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSuppliers.ForeColor = Color.White;
            btnSuppliers.ImageAlign = ContentAlignment.MiddleLeft;
            btnSuppliers.Location = new Point(0, 420);
            btnSuppliers.Name = "btnSuppliers";
            btnSuppliers.Padding = new Padding(20, 0, 0, 0);
            btnSuppliers.Size = new Size(229, 60);
            btnSuppliers.TabIndex = 6;
            btnSuppliers.Text = "   🏭 Fournisseurs";
            btnSuppliers.TextAlign = ContentAlignment.MiddleLeft;
            btnSuppliers.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSuppliers.UseVisualStyleBackColor = true;
            btnSuppliers.Click += BtnSuppliers_Click;
            // 
            // btnPurchases
            // 
            btnPurchases.Cursor = Cursors.Hand;
            btnPurchases.Dock = DockStyle.Top;
            btnPurchases.FlatAppearance.BorderSize = 0;
            btnPurchases.FlatStyle = FlatStyle.Flat;
            btnPurchases.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPurchases.ForeColor = Color.White;
            btnPurchases.ImageAlign = ContentAlignment.MiddleLeft;
            btnPurchases.Location = new Point(0, 360);
            btnPurchases.Name = "btnPurchases";
            btnPurchases.Padding = new Padding(20, 0, 0, 0);
            btnPurchases.Size = new Size(229, 60);
            btnPurchases.TabIndex = 5;
            btnPurchases.Text = "   \U0001f6d2 Achats";
            btnPurchases.TextAlign = ContentAlignment.MiddleLeft;
            btnPurchases.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnPurchases.UseVisualStyleBackColor = true;
            btnPurchases.Click += BtnPurchases_Click;
            // 
            // btnSales
            // 
            btnSales.Cursor = Cursors.Hand;
            btnSales.Dock = DockStyle.Top;
            btnSales.FlatAppearance.BorderSize = 0;
            btnSales.FlatStyle = FlatStyle.Flat;
            btnSales.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSales.ForeColor = Color.White;
            btnSales.ImageAlign = ContentAlignment.MiddleLeft;
            btnSales.Location = new Point(0, 300);
            btnSales.Name = "btnSales";
            btnSales.Padding = new Padding(20, 0, 0, 0);
            btnSales.Size = new Size(229, 60);
            btnSales.TabIndex = 4;
            btnSales.Text = "   💰 Ventes";
            btnSales.TextAlign = ContentAlignment.MiddleLeft;
            btnSales.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSales.UseVisualStyleBackColor = true;
            btnSales.Click += BtnSales_Click;
            // 
            // btnClients
            // 
            btnClients.Cursor = Cursors.Hand;
            btnClients.Dock = DockStyle.Top;
            btnClients.FlatAppearance.BorderSize = 0;
            btnClients.FlatStyle = FlatStyle.Flat;
            btnClients.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnClients.ForeColor = Color.White;
            btnClients.ImageAlign = ContentAlignment.MiddleLeft;
            btnClients.Location = new Point(0, 240);
            btnClients.Name = "btnClients";
            btnClients.Padding = new Padding(20, 0, 0, 0);
            btnClients.Size = new Size(229, 60);
            btnClients.TabIndex = 3;
            btnClients.Text = "   👥 Clients";
            btnClients.TextAlign = ContentAlignment.MiddleLeft;
            btnClients.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnClients.UseVisualStyleBackColor = true;
            btnClients.Click += BtnClients_Click;
            // 
            // btnProducts
            // 
            btnProducts.Cursor = Cursors.Hand;
            btnProducts.Dock = DockStyle.Top;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnProducts.ForeColor = Color.White;
            btnProducts.ImageAlign = ContentAlignment.MiddleLeft;
            btnProducts.Location = new Point(0, 180);
            btnProducts.Name = "btnProducts";
            btnProducts.Padding = new Padding(20, 0, 0, 0);
            btnProducts.Size = new Size(229, 60);
            btnProducts.TabIndex = 2;
            btnProducts.Text = "   📦 Produits";
            btnProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnProducts.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnProducts.UseVisualStyleBackColor = true;
            btnProducts.Click += BtnProducts_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(0, 120);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(20, 0, 0, 0);
            btnDashboard.Size = new Size(229, 60);
            btnDashboard.TabIndex = 1;
            btnDashboard.Text = "   📊 Tableau de Bord";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += BtnDashboard_Click;
            // 
            // logoPanel
            // 
            logoPanel.BackColor = Color.FromArgb(44, 62, 80);
            logoPanel.Controls.Add(pictureBox1);
            logoPanel.Controls.Add(lblAppTitle);
            logoPanel.Dock = DockStyle.Top;
            logoPanel.Location = new Point(0, 0);
            logoPanel.Name = "logoPanel";
            logoPanel.Size = new Size(229, 120);
            logoPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.ErrorImage = Properties.Resources.logo;
            pictureBox1.Image = Properties.Resources.logo1;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(229, 120);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblAppTitle
            // 
            lblAppTitle.BackColor = Color.White;
            lblAppTitle.Dock = DockStyle.Fill;
            lblAppTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(0, 0);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(229, 120);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblAppTitle.Click += lblAppTitle_Click;
            // 
            // mainContentPanel
            // 
            mainContentPanel.BackColor = Color.FromArgb(240, 244, 248);
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.Location = new Point(250, 0);
            mainContentPanel.Name = "mainContentPanel";
            mainContentPanel.Size = new Size(1130, 852);
            mainContentPanel.TabIndex = 1;
            // 
            // MainDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1380, 852);
            Controls.Add(mainContentPanel);
            Controls.Add(sidebarPanel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1200, 650);
            Name = "MainDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Stock";
            WindowState = FormWindowState.Maximized;
            sidebarPanel.ResumeLayout(false);
            logoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }



        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnClients;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnPurchases;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnPurchaseReturns;
        private System.Windows.Forms.Button btnSalesReturns;
        private System.Windows.Forms.Button btnCounterSales;
        private System.Windows.Forms.Button btnCashFlow;
        private System.Windows.Forms.Button btnDataSpreadsheets;
        private System.Windows.Forms.Button btnCommonRepositories;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Panel mainContentPanel;

        #endregion

        private PictureBox pictureBox1;
    }
}