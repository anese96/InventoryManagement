using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
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

        public AjouterVente(FunctionUI functionUI , IService<SalesInvoicesDto> service, IService<SalesInvoiceLineDto> lineService, ProduitRepository produitRepository  ): base(functionUI)
        {
            _produitRepository = produitRepository; 
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            InitializeComponent();
        }

      

        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
            var context = new AppDbContext();
            var selectedCustomer = context.Customers
                             .FirstOrDefault(c => c.Name == cmbClient.Text);
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
                    IdCustomer= selectedCustomer.Id,
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
                MessageBox.Show("Vente ajoutée avec succès");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
                this.DialogResult = DialogResult.OK;
            }

        }

        public async  Task SaveLinesProducts(DataGridView dataGridView, int idSalesInvoice , AppDbContext context)
        {
            foreach (DataGridViewRow row in dgvArticles.Rows)
            {
                if (row.IsNewRow) continue;
                string refProd = row.Cells["RefProduit"].Value?.ToString();
                decimal qteVendue = Convert.ToDecimal(row.Cells["Qte"].Value ?? 0);
                var product = context.Products.FirstOrDefault(p => p.RefProduct == refProd);
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
