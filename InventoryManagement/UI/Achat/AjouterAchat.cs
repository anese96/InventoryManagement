using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Achat
{
    public partial class AjouterAchat : SalePurchaseForm
    {
        private readonly IService<PurchaseDto> _service;
        private readonly IService<PurchaseLineDto> _lineService;
        private readonly FunctionUI _functionUI;
        private readonly ProduitRepository _produitRepository;
        private readonly VendorService _vendorService;
        public AjouterAchat(FunctionUI functionUI, IService<PurchaseDto> service, IService<PurchaseLineDto> lineService, ProduitRepository produitRepository, VendorService vendorService, AppDbContext appDbContext) :
          base(functionUI, appDbContext)
        {
            _produitRepository = produitRepository;
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
           _vendorService = vendorService;
            InitializeComponent();
            BtnAddRow_Click(null, null);
            Title = "🛒 NOUVELLE ACHAT";
            Client_Fournisseur="Fournisseur:";
            dgvArticles.Columns["Tarification"].Visible = false;
        }


        public override List<string> customerNames()
        {
            return _appContext?.Vendors?.Select(c => c.Name).ToList() ?? new List<string>();
        }
        public override void DgvArticles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArticles.Rows[e.RowIndex];
            string colName = dgvArticles.Columns[e.ColumnIndex].Name;


            {
                Product product = null;

                if (colName == "RefProduit")
                {
                    string val = row.Cells["RefProduit"].Value?.ToString()?.ToLower();
                    product = _appContext.Products.FirstOrDefault(p => p.RefProduct.ToLower() == val);
                }
                else if (colName == "Designation")
                {
                    string val = row.Cells["Designation"].Value?.ToString()?.ToLower();
                    product = _appContext.Products.FirstOrDefault(p => p.Designation.ToLower() == val);
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

                    row.Cells["IdProduct"].Value = product.Id;
                    row.Cells["RefProduit"].Value = product.RefProduct;
                    row.Cells["Designation"].Value = product.Designation;

                    // Populate Tarification
                    try
                    {
                        var priceLists = _appContext.PriceLists.Where(pl => pl.ProductId == product.Id).ToList();
                        var options = new System.Collections.Generic.List<PriceOption>();
                        options.Add(new PriceOption { Display = $"Standard ({product.PurchasePrice:N2})", Value = product.PurchasePrice });
                        foreach (var pl in priceLists)
                        {
                            options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N2})", Value = pl.Price });
                        }

                        var tarifCell = (DataGridViewComboBoxCell)row.Cells["Tarification"];
                        tarifCell.DataSource = options;
                        tarifCell.DisplayMember = "Display";
                        tarifCell.ValueMember = "Value";
                        tarifCell.Value = product.PurchasePrice;
                    }
                    catch { }

                    row.Cells["Prix"].Value = product.PurchasePrice?.ToString("N2") ?? "0.00";
                    row.Cells["TVA"].Value = product.Taxe.ToString();
                    row.Cells["Qte"].Value = "1";

                    dgvArticles.CellValueChanged += DgvArticles_CellValueChanged;
                }
            }


            UpdateRowTotal(row);
            CalculateTotals(null, null);
        }

        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!CheckdgvArticlesRows())
                return;
            var context = _appContext;
            var clientName = cmbClient.Text.Trim().ToLower();
            var selectedCustomer = context.Vendors
                .FirstOrDefault(c => c.Name.ToLower() == clientName);

            if (selectedCustomer == null && (decimal)numResteAPayer.Value > 0)
            {
                MessageBox.Show("Veuillez sélectionner un Fournisseur pour les paiements en attente!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var purchaseDto = new PurchaseDto
                {
                    NumberPurchase = txtNumFacture.Text,
                    DatePurchase = dtpDate.Value,
                    IdVendor = selectedCustomer?.Id,
                    TotalWithoutTax = (decimal)numTotalHT.Value,
                    Remise = (decimal)numRemise.Value,
                    TotalWithoutTaxRemise = (decimal)numTotalHTRemise.Value,
                    TotalTax = (decimal)numTotalTVA.Value,
                    TotalPurchase = (decimal)numTotalTTC.Value,
                    PaymentPurchase = (decimal)numMontantPaye.Value,
                    BalancePurchase = (decimal)numResteAPayer.Value,
                    IdCrates = (cbxCaisse.SelectedItem as Crates)?.Id,
                };
                await _service.AddAsync(purchaseDto);
                await SaveLinesProducts(dgvArticles, purchaseDto.Id, context);
                if (selectedCustomer != null)
                {
                    await _vendorService.UpdateBalanceAsync(selectedCustomer.Id, (decimal)numResteAPayer.Value);
                }
                if (selectedCustomer != null)
                {
                    await _vendorService.UpdateTurnoverAsync(selectedCustomer.Id, (decimal)numTotalTTC.Value);
                }
                MessageBox.Show("Achat ajouté avec succès");
                this.DialogResult = DialogResult.OK;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
                this.DialogResult = DialogResult.OK;
            }

        }

        public async Task SaveLinesProducts(DataGridView dataGridView, int idPurchase, AppDbContext context)
        {
            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.IsNewRow) continue;
                string IdProduct = row.Cells["IdProduct"].Value?.ToString();
                decimal qteVendue = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0);
                var product = context.Products.FirstOrDefault(p => p.Id.ToString() == IdProduct);
                _produitRepository.ModifierQty(product.Id, (int)qteVendue, true);
                var purchaseLineDto = new PurchaseLineDto
                {
                    IdProduct = Convert.ToInt32(row.Cells["IdProduct"].Value),
                    RefProduct = row.Cells["RefProduit"].Value?.ToString(),
                    Designation = row.Cells["Designation"].Value?.ToString(),
                    Quantity = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0),
                    Price = Convert.ToDecimal(row.Cells["Prix"].Value ?? 0),
                    Taxe = row.Cells["TVA"].Value?.ToString() ?? "0", // Default tax
                    TotalWithoutTax = Convert.ToDecimal(row.Cells["TotalHT"].Value ?? 0)
                };
                purchaseLineDto.IdPurchase = idPurchase;
                await _lineService.AddAsync(purchaseLineDto);
            }
        }
    }
}
