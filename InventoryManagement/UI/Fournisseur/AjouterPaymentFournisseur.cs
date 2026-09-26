using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using InventoryManagement.Services;
using InventoryManagement.UI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Fournisseur
{
    public class AjouterPaymentFournisseur : AjouterPayment
    {
        private readonly FunctionUI _functionUI;
        private readonly AppDbContext _appContext;

        private readonly IService<PaymentVendorDto> _service;
        private int _idVendor;
        private readonly VendorService _vendorService;
        private readonly CratesRepository _cratesRepository;
        public AjouterPaymentFournisseur(int idVendor, FunctionUI functionUI, IService<PaymentVendorDto> service,
            AppDbContext appDbContext, VendorService vendorService, CratesRepository cratesRepository)
            : base(idVendor, functionUI, null, appDbContext, null, cratesRepository)
        {
                _idVendor = idVendor;
                _functionUI = functionUI;
                _service = service;
                _appContext = appDbContext;
                _vendorService = vendorService;
            _cratesRepository = cratesRepository;
        }

        public override async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var paymentvendorDto = new PaymentVendorDto
                {
                    IdVendor = _idVendor,
                    NumberPayment = txtNumberPayment.Text.Trim(),
                    DatePayment = dtpDate.Value,
                    Payment = decimal.Parse(txtAmount.Text.Trim()),
                    IdCrates = (int)cbCaisse.SelectedValue
                };
                await _service.AddAsync(paymentvendorDto);
                await _vendorService.UpdateBalanceAsync(_idVendor, -(paymentvendorDto.Payment.Value));
                await _cratesRepository.AddMoney((int)cbCaisse.SelectedValue, -(decimal.Parse(txtAmount.Text.Trim())));
                MessageBox.Show("Payment ajouté avec succès");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
        }
    }
}
