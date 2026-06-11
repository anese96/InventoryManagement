using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class PaymentVendorRepository : IRepository<PaymentVendorDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public PaymentVendorRepository(AppDbContext appContext, AddEntityBD addEntityBD)
        {
            _appContext = appContext;
            _addEntityBD = addEntityBD;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async  Task<List<PaymentVendorDto>> GetAll()
        {
            var vendors =  _appContext.paymentVendors.ToList();
            var vendorDtos = vendors.Select(vendor => new PaymentVendorDto
            {
                IdVendor = vendor.IdVendor,
                IdCrates = vendor.IdCrates,
                NumberPayment = vendor.NumberPayment,
                DatePayment = vendor.DatePayment,
                Payment = vendor.Payment
            }).ToList();
            if (vendorDtos != null && vendorDtos.Count > 0)
            {
                return await Task.FromResult(vendorDtos);
            }
            else
            {
                throw new ArgumentException("No payments found.");

            }
        }

        public Task<PaymentVendorDto> GetById(int Id)
        {
            var vendor = _appContext.paymentVendors.
                Where(x => x.IdVendor == Id).FirstOrDefault();
            if (vendor == null)
            {
                throw new ArgumentException("Payment not found.");
            }
            var vendorDto = new PaymentVendorDto    
            {
                IdVendor = vendor.IdVendor,
                IdCrates = vendor.IdCrates,
                NumberPayment = vendor.NumberPayment,
                DatePayment = vendor.DatePayment,
                Payment = vendor.Payment
            };
            return Task.FromResult(vendorDto);
        }

        public async Task Insert(PaymentVendorDto entity)
        {
            var payment = new Data.Models.PaymentVendor
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                IdVendor = entity.IdVendor,
                NumberPayment = entity.NumberPayment,
                DatePayment = entity.DatePayment,
                Payment = entity.Payment,
                IdCrates = entity.IdCrates
            };
            await _appContext.paymentVendors.AddAsync(payment);
            await _appContext.SaveChangesAsync();
        }

        public Task Update(PaymentVendorDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
