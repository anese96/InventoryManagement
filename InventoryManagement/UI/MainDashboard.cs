using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.UI.Produit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using InventoryManagement.UI.Client;
using InventoryManagement.UI.Fournisseur;
using InventoryManagement.UI.Vente;
using InventoryManagement.UI.VentesComptoir;
using InventoryManagement.UI.Achat;
using InventoryManagement.Data;
using InventoryManagement.UI.Dashboard;

namespace InventoryManagement.UI
{
    public partial class MainDashboard : Form
    {
       
        private Button currentActiveButton;
        private readonly IFormManager _formFactory;
        private readonly AppDbContext _appContext;
        private Panel currentFormPanel;
        private readonly GetTotal _getTotal;
        public MainDashboard(IFormManager formFactory, AppDbContext appDbContext, GetTotal getTotal )
        {
            InitializeComponent();
            _formFactory = formFactory;
            _appContext = appDbContext;
            _getTotal = getTotal;
            ShowDashboardHome();
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }
        private void BtnCommonRepositories_Click(object sender, EventArgs e)
        {
            ShowReferentielsDashboard();
        }

        private void ShowReferentielsDashboard()
        {
            // Clear current content
            if (currentFormPanel != null)
            {
                mainContentPanel.Controls.Remove(currentFormPanel);
                currentFormPanel.Dispose();
            }

            // Create referentials dashboard panel
            currentFormPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248)
            };

            // Add title
            Label titleLabel = new Label
            {
                Text = "Gestion des Référentiels",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            currentFormPanel.Controls.Add(titleLabel);

            // Create cards for Categorie, Unite, Nature, Marque
            int cardWidth = 220;
            int cardHeight = 150;
            int spacing = 30;
            int startX = 30;
            int startY = 100;

            var types = new[] 
            {
                new { Type = InventoryManagement.Repositorys.ReferentielType.Categorie, Name = "Catégories", Icon = "📁", Color = Color.FromArgb(52, 152, 219) },
                new { Type = InventoryManagement.Repositorys.ReferentielType.Unite, Name = "Unités", Icon = "⚖️", Color = Color.FromArgb(46, 204, 113) },
                new { Type = InventoryManagement.Repositorys.ReferentielType.Nature, Name = "Natures", Icon = "🌱", Color = Color.FromArgb(155, 89, 182) },
                new { Type = InventoryManagement.Repositorys.ReferentielType.Marque, Name = "Marques", Icon = "🏷️", Color = Color.FromArgb(230, 126, 34) },
                new { Type = InventoryManagement.Repositorys.ReferentielType.Caisse, Name = "Caisses", Icon = "🏦", Color = Color.FromArgb(231, 76, 60) }
            };

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                int x = startX + i * (cardWidth + spacing);

                Panel card = CreateDashboardCard(
                    t.Name,
                    t.Icon,
                    " " + t.Name.ToLower(),
                    t.Color,
                    new Point(x, startY),
                    new Size(cardWidth, cardHeight),
                    () => OpenReferentielForm(t.Type, t.Name)
                );
                currentFormPanel.Controls.Add(card);
            }

            mainContentPanel.Controls.Add(currentFormPanel);
        }

        private async void OpenReferentielForm(InventoryManagement.Repositorys.ReferentielType type, string title)
        {
            try
            {
                using (var scope = Program.ServiceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<InventoryManagement.Data.AppDbContext>();
                    var audit = scope.ServiceProvider.GetRequiredService<InventoryManagement.Data.Entity.AddEntityBD>();
                    
                    var repo = new InventoryManagement.Repositorys.ReferentielRepository(db, audit, type);
                    var data = await repo.GetAll();

                    var listForm = new GenericListForm<InventoryManagement.Data.DTO.ReferentielDto>(data, null, "Gestion des " + title);
                    
                    listForm.OnAdd += async (dto) => {
                        try {
                             await repo.Insert(dto);
                            // Optional: Reload list to get IDs if needed
                        } catch(Exception ex) {
                            MessageBox.Show("Erreur lors de l'ajout : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    listForm.OnEdit += async (dto) => {
                        try {
                            await repo.Update(dto, dto.Id);
                        } catch(Exception ex) {
                            MessageBox.Show("Erreur lors de la modification : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    listForm.OnDelete += async (dto) => {
                        try {
                            await repo.Delete(dto.Id);
                            return true;
                        } catch(Exception ex) {
                            MessageBox.Show("Erreur lors de la suppression : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    };

                    listForm.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur d'ouverture : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetActiveButton(Button button)
        {
            // Reset previous active button
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.FromArgb(52, 73, 94);
                currentActiveButton.ForeColor = Color.White;
            }

            // Set new active button
            currentActiveButton = button;
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.FromArgb(41, 128, 185);
                currentActiveButton.ForeColor = Color.White;
            }
        }

        private void LoadFormInPanel(Form form)
        {
            // Clear current content
            if (currentFormPanel != null)
            {
                mainContentPanel.Controls.Remove(currentFormPanel);
                currentFormPanel.Dispose();
            }

            // Create new panel for form
            currentFormPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248)
            };

            // Configure and add form
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            currentFormPanel.Controls.Add(form);
            form.Show();

            mainContentPanel.Controls.Add(currentFormPanel);
        }
        private void BtnProducts_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnProducts);
            LoadFormInPanel(new ListeProduits(_formFactory, _appContext));
        }
        private void BtnCategories_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }   
        private void BtnDataSpreadsheets_Click(object sender, EventArgs e)
        {
           // throw new NotImplementedException();///tt
           SetActiveButton(btnDataSpreadsheets);
           LoadFormInPanel(new InventoryManagement.UI.Dashboard.Dashboard());
        }
        private void BtnCashFlow_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        //private void BtnCounterSales_Click(object sender, EventArgs e)
        //{
           
        //}
        private void BtnSalesReturns_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void BtnPurchaseReturns_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void BtnSuppliers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSuppliers);
            LoadFormInPanel(new ListeFournisseur(_formFactory, _appContext));
        }
        private void BtnPurchases_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPurchases);          
            LoadFormInPanel(new ListeAchats(_formFactory, _appContext));

        }
        private void BtnSales_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSales);
            LoadFormInPanel(new ListeVentes(_formFactory, _appContext));
           
        }
        private void BtnClients_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnClients);
            LoadFormInPanel(new ListeClient(_formFactory, _appContext));
            
        }
        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboardHome();
        }

        private void ShowDashboardHome()
        {
            // Clear current content
            if (currentFormPanel != null)
            {
                mainContentPanel.Controls.Remove(currentFormPanel);
                currentFormPanel.Dispose();
            }

            // Create dashboard home panel
            currentFormPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 248)
            };

            // Add welcome label
            Label welcomeLabel = new Label
            {
                Text = "Tableau de Bord",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            currentFormPanel.Controls.Add(welcomeLabel);

            // Create dashboard cards
           CreateDashboardCards(currentFormPanel);

            mainContentPanel.Controls.Add(currentFormPanel);
        }
        private async void CreateDashboardCards(Panel parentPanel)
        {
            int cardWidth = 280;
            int cardHeight = 180;
            int spacing = 30;
            int startX = 30;
            int startY = 100;

            // Products Card
            string productCount = await  _getTotal.GetTotalProductCount();
            Panel productsCard = CreateDashboardCard(
                "Produits",
                productCount,
                "Articles en stock",
                Color.FromArgb(52, 152, 219),
                new Point(startX, startY),
                new Size(cardWidth, cardHeight),
                () => BtnProducts_Click(null, EventArgs.Empty)
            );
            parentPanel.Controls.Add(productsCard);

            // Sales Card
            string TotalVents = await _getTotal.GetTotalVents();
            string nomMois = DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.CurrentCulture);
            Panel salesCard = CreateDashboardCard(
                "Ventes",
               TotalVents,
                "Ventes de " + nomMois,
                Color.FromArgb(155, 89, 182),
                new Point(startX + (cardWidth + spacing), startY),
                new Size(cardWidth, cardHeight),
                () => BtnSales_Click(null, EventArgs.Empty)
            );
            parentPanel.Controls.Add(salesCard);

            // Clients Card
            string clientCount = await _getTotal.GetTotaClientCount();
            Panel clientsCard = CreateDashboardCard(
                "Clients",
                 clientCount,
                "Clients actifs",
                Color.FromArgb(46, 204, 113),
                new Point(startX + 2 * (cardWidth + spacing), startY),
                new Size(cardWidth, cardHeight),
                () => BtnClients_Click(null, EventArgs.Empty)
            );
            parentPanel.Controls.Add(clientsCard);



            // Purchases Card
            string TotalAchats = await _getTotal.GetTotalAchats();
            Panel purchasesCard = CreateDashboardCard(
                "Achats",
                TotalAchats,
                "Achats de " + nomMois,
                Color.FromArgb(230, 126, 34),
                new Point(startX + 3 * (cardWidth + spacing), startY),
                new Size(cardWidth, cardHeight),
                () => BtnPurchases_Click(null, EventArgs.Empty)
            );
            parentPanel.Controls.Add(purchasesCard);

            // Create Quick Actions Buttons
            CreateQuickActions(parentPanel);
        }

        private void CreateQuickActions(Panel parentPanel)
        {
            int buttonWidth = 220;
            int buttonHeight = 60;
            int spacingX = 30;
            int spacingY = 20;
            int startX = 30;
            int startY = 320;

            Label sectionLabel = new Label
            {
                Text = "Accès Rapide",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(startX, startY - 40)
            };
            parentPanel.Controls.Add(sectionLabel);

            // Definitions of actions ListButton
            var actions = new[]
            {
                new { Name = "➕ Ajouter un Produit", Color = Color.FromArgb(52, 152, 219), Action = (Action)(() => _formFactory.Open<AjouterProduit>()   ) },
                new { Name = "➕ Ajouter Client", Color = Color.FromArgb(46, 204, 113), Action = (Action)(() => _formFactory.Open<AjouterClient>() ) },
                new { Name = "➕ Ajouter Fournisseur", Color = Color.FromArgb(230, 126, 34), Action = (Action)(() =>_formFactory.Open<AjouterFournisseur>() ) },
                new { Name = "💰 Ajouter Vente", Color = Color.FromArgb(155, 89, 182), Action = (Action)(() => _formFactory.Open<AjouterVente>()) },
                new { Name = "🏪 Vente Comptoir", Color = Color.FromArgb(231, 76, 60), Action = (Action)(() => _formFactory.Open<AjouterVentesComptoir>()) },
                new { Name = "🛒 Ajouter Achat", Color = Color.FromArgb(243, 156, 18), Action = (Action)(() =>_formFactory.Open<AjouterAchat>()) }
            };

            for (int i = 0; i < actions.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;

                int x = startX + col * (buttonWidth + spacingX);
                int y = startY + row * (buttonHeight + spacingY);

                // Check iteration variable capture in closure? 
                // In C# 5+ foreach loop variable is fresh, but for loop index is not. 
                // We must use a local variable for the closure.
                var actionItem = actions[i];

                Button btn = new Button
                {
                    Text = actionItem.Name,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    BackColor = actionItem.Color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(x, y),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => actionItem.Action();

                // Hover effect
                btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(actionItem.Color);
                btn.MouseLeave += (s, e) => btn.BackColor = actionItem.Color;

                parentPanel.Controls.Add(btn);
            }
        }
        private Panel CreateDashboardCard(string title, string value, string subtitle, Color accentColor, Point location, Size size, Action onClickAction)
        {
            Panel card = new Panel
            {
                Location = location,
                Size = size,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(new Rectangle(0, 0, card.Width, card.Height));
                    card.Region = new Region(path);
                }
            };

            // Title
            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            card.Controls.Add(titleLabel);

            // Value
            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 55)
            };
            card.Controls.Add(valueLabel);

            // Subtitle
            Label subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = false,
                Size = new Size(card.Width - 40, 40),
                Location = new Point(20, card.Height - 45)
            };
            card.Controls.Add(subtitleLabel);

            // Hover effects
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(248, 249, 250);
            };
            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
            };

            // Card click handler
            card.Click += (s, e) => onClickAction();

            // Make child controls clickable by propagating their click to the card's action
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => onClickAction();
                ctrl.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 249, 250);
                ctrl.MouseLeave += (s, e) => card.BackColor = Color.White;
            }

            return card;
        }
        

    }
}
