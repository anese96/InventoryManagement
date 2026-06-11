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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Achat
{
    public partial class ModifierAchat : SalePurchaseForm
    {
        private readonly IService<PurchaseDto> _service;
        private readonly IService<PurchaseLineDto> _lineService;
        private readonly ProduitRepository _produitRepository;
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appContext;
        private int _id;
        public ModifierAchat(int id, FunctionUI functionUI, IService<PurchaseDto> service, IService<PurchaseLineDto> lineService,
            ProduitRepository produitRepository, AppDbContext appDbContext) : base(functionUI, appDbContext)
        {
          
            cmbClient.ReadOnly = true;
            cbxCaisse.Enabled = false;
            _id = id;
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            _produitRepository = produitRepository;
                _appContext = appDbContext;
            InitializeComponent(); 
            Visibility = false;
            LoadData();
            Title = "🛒 MODIFIER ACHAT";
            Client_Fournisseur = "Fournisseur:";
            dgvArticles.Columns["Tarification"].Visible = false;
        }
        private void LoadData()
        {

            _service.GetAsyncById(_id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var purchase = task.Result;
                    if (purchase != null)
                    {
                        var db = _appContext;
                        var vendor = db.Vendors.Find(purchase.IdVendor);
                        if (vendor != null)
                        {
                            cmbClient.Text = vendor.Name;
                        }
                        txtNumFacture.Text = purchase.NumberPurchase;
                        dtpDate.Value = purchase.DatePurchase;
                        numTotalHT.Value = purchase.TotalWithoutTax ?? 0;
                        numRemise.Value = purchase.Remise ?? 0;
                        numTotalHTRemise.Value = purchase.TotalWithoutTaxRemise ?? 0;
                        numTotalTVA.Value = purchase.TotalTax ?? 0;
                        numTotalTTC.Value = purchase.TotalPurchase ?? 0;
                        numMontantPaye.Value = purchase.PaymentPurchase ?? 0;
                        numResteAPayer.Value = purchase.BalancePurchase ?? 0;
                        cbxCaisse.SelectedValue = purchase.IdCrates ?? (object)DBNull.Value;
                    }
                    else
                    {
                        MessageBox.Show("Facture non trouvée.");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement du  facture.");
                    this.Close();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            LoadLineData();
           
        }


        public async Task LoadLineData()
        {
            try
            {
                var result = await _lineService.GetAllAsyncs();

                var lines = result
                            .Where(l => l.IdPurchase == _id)
                            .ToList();

                dgvArticles.Rows.Clear();

                foreach (var line in lines)
                {
                    dgvArticles.Rows.Add(
                        line.IdProduct,
                        line.RefProduct,
                        line.Designation,
                        null,
                        line.Quantity.ToString("N2"),
                        line.Price.ToString("N2"),
                        null,
                        line.Taxe,
                        line.TotalWithoutTax.ToString("N2")
                    );
                }

                CalculateTotals(null, null);
            }
            catch
            {
                MessageBox.Show("Erreur lors du chargement des lignes de produit.");
            }
        }

        public override List<string> customerNames()
        {
            return _appContext?.Vendors?.Select(c => c.Name).ToList() ?? new List<string>();
        }
        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
           
        }
    }
}
