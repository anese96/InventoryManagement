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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.UI.Vente
{
    public partial class AjouterVente : SalePurchaseForm
    {
        private readonly IService<SalesInvoicesDto> _service;
        private readonly IService<SalesInvoiceLineDto> _lineService;
        private readonly FunctionUI _functionUI;
        private readonly ProduitRepository _produitRepository;
        private readonly ClientService _clientService;
        private readonly CratesRepository _cratesRepository;


        public AjouterVente(FunctionUI functionUI , IService<SalesInvoicesDto> service, IService<SalesInvoiceLineDto> lineService, ProduitRepository produitRepository , ClientService clientService, AppDbContext appDbContext , CratesRepository cratesRepository) :
            base(functionUI, appDbContext)
        {
            _produitRepository = produitRepository; 
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            _clientService = clientService;
            _cratesRepository = cratesRepository;
            InitializeComponent();
            BtnAddRow_Click(null, null);
            Title = "💰 NOUVELLE VENTE";
            Client_Fournisseur="Client:";
            Visibility = true;
        }

     

        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!CheckdgvArticlesRows())
                return;
            var context = _appContext;
            var clientName = cmbClient.Text.Trim().ToLower();
            var selectedCustomer = context.Customers
                .FirstOrDefault(c => c.Name.ToLower() == clientName);

            if (selectedCustomer == null && (decimal)numResteAPayer.Value > 0)
            {
                MessageBox.Show("Veuillez sélectionner un client pour les paiements en attente!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var salesInvoicesDto = new SalesInvoicesDto
                {
                    NumberInvoice= txtNumFacture.Text,
                    DateInvoice = dtpDate.Value,
                    IdCustomer= selectedCustomer?.Id,
                    TotalWithoutTax= (decimal)numTotalHT.Value,
                    Remise = (decimal)numRemise.Value,
                    TotalWithoutTaxRemise = (decimal)numTotalHTRemise.Value,
                    TotalTax = (decimal)numTotalTVA.Value,
                    TotalInvoice = (decimal)numTotalTTC.Value,
                    PaymentInvoice = (decimal)numMontantPaye.Value,
                    BalanceInvoice = (decimal)numResteAPayer.Value,
                    IdCrates = (cbxCaisse.SelectedItem as Crates)?.Id,
                };
                await _service.AddAsync(salesInvoicesDto);
                await SaveLinesProducts(dgvArticles, salesInvoicesDto.Id, context);
                await _cratesRepository.AddMoney((cbxCaisse.SelectedItem as Crates).Id, (decimal)numMontantPaye.Value);
                if (selectedCustomer != null)
                {
                    await _clientService.UpdateBalanceAsync(selectedCustomer.Id, (decimal)numResteAPayer.Value);
                    await _clientService.UpdateTurnoverAsync(selectedCustomer.Id, (decimal)numTotalTTC.Value);
                }
                MessageBox.Show("Vente ajoutée avec succès");
                this.DialogResult = DialogResult.OK;
             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public override List<string> customerNames()
        {
          return _appContext?.Customers?.Select(c => c.Name).ToList() ?? new List<string>();
        }
        public async Task SaveLinesProducts(DataGridView dataGridView, int idSalesInvoice, AppDbContext context)
        {
            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.IsNewRow) continue;
                string IdProduct = row.Cells["IdProduct"].Value?.ToString();
                decimal qteVendue = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0);
                var product = context.Products.FirstOrDefault(p => p.Id.ToString() == IdProduct);
                if (product == null)
                    continue;
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
