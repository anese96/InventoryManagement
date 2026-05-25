using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Produit
{
    public partial class ModifierProduit : AjouterProduit
    {
        private readonly FunctionUI _functionUI;
        private readonly IService<ProduitDto> _service;
        private int _id;
        private readonly AppDbContext _appContext;
        
        public ModifierProduit(int id , FunctionUI functionUI, IService<ProduitDto> service, AppDbContext appContext):base(functionUI, service, appContext)
        {
            _id = id;
            _functionUI = functionUI;
            _appContext = appContext;
            _service = service;
            InitializeComponent();
            InitializeCustomComponents();
            LoadProduitData();
            GetPriceLists(_id);

        }


        private void LoadProduitData()
        {
            _service.GetAsyncById(_id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var produit = task.Result;
                    if (produit != null)
                    {
                            txtRef.Text = produit.RefProduct ;
                            txtDesignation.Text = produit.Designation;
                            cbCategory.SelectedValue =produit.CategoryId ?? -1;
                            cbUnit.SelectedValue = produit.UnitId ?? -1;
                            cbBrand.SelectedValue = produit.MarqueId ?? -1;
                            cbNature.SelectedValue = produit.NatureId ?? -1;                         
                            cbTaxe.Text = produit.Taxe;
                            txtBarCode.Text = produit.BarCode;
                            txtPurchasePrice.Text = produit.PurchasePrice?.ToString("F2");
                            txtSalesPrice.Text = produit.SalesPrice?.ToString("F2");
                            txtStock.Text = produit.StockQuantity?.ToString("F2");
                            txtAlertQty.Text = produit.QtyAlert?.ToString("F2");
                            txtColisage.Text = produit.Colisage?.ToString();
                    }
                    
                    else
                    {
                        MessageBox.Show("Produit non trouvé.");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement du produit.");
                    this.Close();
                }
            });
        }

        public void GetPriceLists(int IdProduct)
        {
           
            var priceLists = _appContext.PriceLists.Where(p => p.ProductId == IdProduct).ToList();
            dgvPriceLists.Rows.Clear();
            foreach (var price in priceLists)
            {
                dgvPriceLists.Rows.Add(price.Name, price.Price?.ToString("F2"));
            }
        }

        public void InitializeCustomComponents()
        {
            this.Text = "Modifier Produit";
            this.Size = new Size(1250, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;


        }
        public  override async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                await _service.UpdateAsync(new ProduitDto
                {
                    RefProduct = txtRef.Text,
                    Designation = txtDesignation.Text,
                    Taxe = cbTaxe.Text,
                    BarCode = txtBarCode.Text,
                    CategoryId = (int?)cbCategory.SelectedValue,
                    UnitId = (int?)cbUnit.SelectedValue,
                    MarqueId = (int?)cbBrand.SelectedValue,
                    NatureId = (int?)cbNature.SelectedValue,
                    PurchasePrice = _functionUI.ParseDecimal(txtPurchasePrice.Text),
                    SalesPrice = _functionUI.ParseDecimal(txtSalesPrice.Text),
                    StockQuantity = _functionUI.ParseDecimal(txtStock.Text),
                    QtyAlert = _functionUI.ParseDecimal(txtAlertQty.Text),
                    Colisage = (int?)_functionUI.ParseDecimal(txtColisage.Text)
                }, _id);

                SavePriceList(dgvPriceLists, _id);
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);

                this.DialogResult = DialogResult.None; 
            }

        }

        public override async Task SavePriceList(DataGridView dataGridView, int IdProduct)
        {
           
            // 3. Sync Price Lists (Remove and Re-add)
            var existingPrices = _appContext.PriceLists.Where(p => p.ProductId == IdProduct);
            _appContext.PriceLists.RemoveRange(existingPrices);

            foreach (DataGridViewRow row in dgvPriceLists.Rows)
            {
                if (row.IsNewRow) continue;
                string name = row.Cells[0].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    _appContext.PriceLists.Add(new PriceLists
                    {
                        ProductId = IdProduct,
                        Name = name,
                        Price = Convert.ToDecimal(row.Cells[1].Value?.ToString())
                    });
                }
            }

            _appContext.SaveChanges();
        }
    }
}
