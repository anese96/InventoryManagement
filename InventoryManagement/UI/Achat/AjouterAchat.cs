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
        private readonly IService<SalesInvoicesDto> _service;
        private readonly IService<SalesInvoiceLineDto> _lineService;
        private readonly FunctionUI _functionUI;
        private readonly ProduitRepository _produitRepository;
        private readonly ClientService _clientService;
        public AjouterAchat(FunctionUI functionUI, IService<SalesInvoicesDto> service, IService<SalesInvoiceLineDto> lineService, ProduitRepository produitRepository, ClientService clientService, AppDbContext appDbContext) :
          base(functionUI, appDbContext)
        {
            _produitRepository = produitRepository;
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            _clientService = clientService;
            InitializeComponent();
            BtnAddRow_Click(null, null);
            Title = "🛒 NOUVELLE ACHAT";
            Client_Fournisseur="Fournisseur:";
            dgvArticles.Columns["Tarification"].Visible = false;
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

        public override void BtnSave_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
