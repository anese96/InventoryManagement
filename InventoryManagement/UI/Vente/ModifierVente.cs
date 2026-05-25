using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.UI.Vente
{
    public partial class ModifierVente : SalePurchaseForm
    {
        private readonly IService<SalesInvoicesDto> _service;
        private readonly IService<SalesInvoiceLineDto> _lineService;
        private readonly ProduitRepository _produitRepository;
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appContext;
        private int _id;
        public ModifierVente(int id, FunctionUI functionUI, IService<SalesInvoicesDto> service, IService<SalesInvoiceLineDto> lineService, ProduitRepository produitRepository, AppDbContext appDbContext) : base(functionUI, appDbContext  )
        {
            Title = "MODIFIER VENTE";
            cmbClient.ReadOnly = true;
            cbxCaisse.Enabled = false;
            _id = id;
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            _produitRepository = produitRepository;
            _appContext = appDbContext;
            InitializeComponent();
        
            LoadData();

        }

        private void LoadData()
        {


            _service.GetAsyncById(_id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var salesInvoices = task.Result;
                    if (salesInvoices != null)
                    {
                        var db = _appContext;
                        var customer = db.Customers.Find(salesInvoices.IdCustomer);
                        if (customer != null)
                        {
                            cmbClient.Text = customer.Name;
                        }
                        txtNumFacture.Text = salesInvoices.NumberInvoice;
                        dtpDate.Value = salesInvoices.DateInvoice;
                        numTotalHT.Value = salesInvoices.TotalWithoutTax ?? 0;
                        numRemise.Value = salesInvoices.Remise ?? 0;
                        numTotalHTRemise.Value = salesInvoices.TotalWithoutTaxRemise ?? 0;
                        numTotalTVA.Value = salesInvoices.TotalTax ?? 0;
                        numTotalTTC.Value = salesInvoices.TotalInvoice ?? 0;
                        numMontantPaye.Value = salesInvoices.PaymentInvoice ?? 0;
                        numResteAPayer.Value = salesInvoices.BalanceInvoice ?? 0;
                        cbxCaisse.SelectedValue = salesInvoices.IdCrates ?? (object)DBNull.Value;
                    }
                    else
                    {
                        MessageBox.Show("Facture non trouvée.");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement du produit.");
                    this.Close();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            LoadLineData();
        }

        public void LoadLineData()
        {
            _lineService.GetAllAsyncs().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var salesInvoiceslines = task.Result.Where(l => l.IdSalesInvoice == _id).ToList();
                    if (salesInvoiceslines != null && salesInvoiceslines.Count > 0)
                    {
                        var db = _appContext;   
                        {
                            var productIds = salesInvoiceslines.Select(l => l.IdProduct).Distinct().ToList();
                            var products = db.Products.AsNoTracking()
                                .Where(p => productIds.Contains(p.Id))
                                .Include(p => p.PriceLists)
                                .ToList();

                            foreach (var line in salesInvoiceslines)
                            {
                                var product = products.FirstOrDefault(p => p.Id == line.IdProduct);
                                string qtePack = "";
                                if (product != null && product.Colisage.HasValue && product.Colisage.Value > 0)
                                {
                                    qtePack = (line.Quantity / product.Colisage.Value).ToString("G29");
                                }

                                int rowIndex = dgvArticles.Rows.Add(
                                    line.IdProduct,
                                    line.RefProduct,
                                    line.Designation,
                                    null, // Tarification
                                    line.Quantity.ToString("N2"),
                                    line.Price.ToString("N2"),
                                    qtePack,
                                    line.Taxe,
                                    line.TotalWithoutTax.ToString("N2")
                                );

                                if (product != null)
                                {
                                    var options = new List<PriceOption>();
                                    options.Add(new PriceOption { Display = $"Standard ({product.SalesPrice:N2})", Value = product.SalesPrice });
                                    if (product.PriceLists != null)
                                    {
                                        foreach (var pl in product.PriceLists)
                                        {
                                            options.Add(new PriceOption { Display = $"{pl.Name} ({pl.Price:N2})", Value = pl.Price });
                                        }
                                    }

                                    var row = dgvArticles.Rows[rowIndex];
                                    var tarifCell = (DataGridViewComboBoxCell)row.Cells["Tarification"];
                                    tarifCell.DataSource = options;
                                    tarifCell.DisplayMember = "Display";
                                    tarifCell.ValueMember = "Value";
                                    tarifCell.Value = line.Price;
                                }
                            }
                        }
                        CalculateTotals(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Aucune ligne de facture trouvée pour cette facture.");
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement des lignes de facture.");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
            //if (!CheckdgvArticlesRows())
            //    return;
            //var context = _appContext;
            //var clientName = cmbClient.Text.Trim().ToLower();
            //var selectedCustomer = context.Customers
            //    .FirstOrDefault(c => c.Name.ToLower() == clientName);

            //if (selectedCustomer == null && (decimal)numResteAPayer.Value > 0)
            //{
            //    MessageBox.Show("Veuillez sélectionner un client pour les paiements en attente!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //try
            //{
            //    await _service.UpdateAsync(new SalesInvoicesDto
            //    {
            //        IdCustomer = selectedCustomer?.Id,
            //        NumberInvoice = txtNumFacture.Text,
            //        DateInvoice = dtpDate.Value,
            //        TotalWithoutTax = (decimal)numTotalHT.Value,
            //        Remise = (decimal)numRemise.Value,
            //        TotalWithoutTaxRemise = (decimal)numTotalHTRemise.Value,
            //        TotalTax = (decimal)numTotalTVA.Value,
            //        TotalInvoice = (decimal)numTotalTTC.Value,
            //        PaymentInvoice = (decimal)numMontantPaye.Value,
            //        BalanceInvoice = (decimal)numResteAPayer.Value,
            //        // IdCrates = (cbxCaisse.SelectedItem as Crates)?.Id,

            //    }, _id);
            //    await ChangeLinesProducts(dgvArticles, _id);
            //    MessageBox.Show("Vente ajoutée avec succès");
            //    this.DialogResult = DialogResult.OK;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            //    this.DialogResult = DialogResult.OK;
            //}
        }

        private async Task ChangeLinesProducts(DataGridView dgvArticles, int id)
        {
            var context = _appContext;

            var salesInvoiceLines = (await _lineService.GetAllAsyncs())
                .Where(l => l.IdSalesInvoice == id)
                .ToList();
            
            if (salesInvoiceLines.Any())
            {
                foreach (var line in salesInvoiceLines)
                {
                    _produitRepository.ModifierQty(line.IdProduct, (int)line.Quantity, true);
                    await _lineService.DeleteAsynct(id);
                }
                await context.SaveChangesAsync();
                await SaveLinesProducts(dgvArticles, _id, context);

            }
        }

        private async Task SaveLinesProducts(DataGridView dgvArticles, int idSalesInvoice, AppDbContext context)
        {
            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.IsNewRow) continue;
                string IdProduct = row.Cells["IdProduct"].Value?.ToString();
                decimal qteVendue = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0);
                var product = context.Products.FirstOrDefault(p => p.Id.ToString() == IdProduct);
                _produitRepository.ModifierQty(product.Id, (int)qteVendue, false);
                var salesInvoiceLineDto = new SalesInvoiceLineDto
                {
                    IdProduct = Convert.ToInt32(row.Cells["IdProduct"].Value),
                    RefProduct = row.Cells["RefProduit"].Value?.ToString(),
                    Designation = row.Cells["Designation"].Value?.ToString(),
                    Quantity = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0),
                    Price = Convert.ToDecimal(row.Cells["Prix"].Value ?? 0),
                    Taxe = row.Cells["TVA"].Value?.ToString() ?? "0", // Default tax
                    TotalWithoutTax = Convert.ToDecimal(row.Cells["TotalHT"].Value ?? 0)
                };
                salesInvoiceLineDto.IdSalesInvoice = idSalesInvoice;
                await _lineService.AddAsync(salesInvoiceLineDto);
            }
        }
    }
}