using InventoryManagement.Data.DTO;
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
    public partial class ModifierVente : SalePurchaseForm
    {
        private readonly IService<SalesInvoicesDto> _service;
        private readonly IService<SalesInvoiceLineDto> _lineService;
        private readonly ProduitRepository _produitRepository;
        private readonly FunctionUI _functionUI;
        private int _id;
        public ModifierVente(int id, FunctionUI functionUI,IService<SalesInvoicesDto> service, IService<SalesInvoiceLineDto> lineService, ProduitRepository produitRepository):base(functionUI)
        {
            _id = id;
            _functionUI = functionUI;
            _service = service;
            _lineService = lineService;
            _produitRepository = produitRepository;
            InitializeComponent();
            LoadProduitData();
        }

        private void LoadProduitData()
        {
            _service.GetAsyncById(_id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var salesInvoices = task.Result;
                    if (salesInvoices != null)
                    {

                        //cmbClient.Text = salesInvoices.;
                        cmbClient.Text = salesInvoices.NumberInvoice;
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

        public override void BtnSave_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
