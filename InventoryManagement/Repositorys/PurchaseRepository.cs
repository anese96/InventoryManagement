using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagement.Repositorys
{
    public class PurchaseRepository : IRepository<PurchaseDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public PurchaseRepository(AppDbContext appContext,AddEntityBD addEntityBD)
        {
            _addEntityBD = addEntityBD;
            _appContext = appContext;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PurchaseDto>> GetAll()
        {
            return await Task.FromResult(_appContext.Purchases.Select(s => new PurchaseDto
            {
                Id = s.Id,
                NumberPurchase = s.NumberPurchase,
                DatePurchase = s.DatePurchase,
                IdVendor = s.IdVendor,
                TotalWithoutTax = s.TotalWithoutTax,
                Remise = s.Remise,
                TotalWithoutTaxRemise = s.TotalWithoutTaxRemise,
                TotalTax = s.TotalTax,
                TotalPurchase = s.TotalPurchase,
                PaymentPurchase = s.PaymentPurchase,
                BalancePurchase = s.BalancePurchase,
                IdCrates = s.IdCrates
            }).ToList());
        }

        public async Task<PurchaseDto> GetById(int Id)
        {
            var purchasse = await _appContext.Purchases.FindAsync(Id);
            if (purchasse == null)
            {
                throw new ArgumentException("Client not found.");
            }
            return new PurchaseDto
            {   NumberPurchase=purchasse.NumberPurchase,
                DatePurchase=purchasse.DatePurchase,
                IdVendor=purchasse.IdVendor,
                TotalWithoutTax=purchasse.TotalWithoutTax,
                Remise=purchasse.Remise,   
                TotalWithoutTaxRemise=purchasse.TotalWithoutTaxRemise,
                TotalTax=purchasse.TotalTax,
                TotalPurchase=purchasse.TotalPurchase,
                PaymentPurchase=purchasse.PaymentPurchase,
                BalancePurchase=purchasse.BalancePurchase,
            };

        }

        public async Task Insert(PurchaseDto entity)
        {
            var purchase = new Purchase
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),

                NumberPurchase = entity.NumberPurchase,  
                DatePurchase=entity.DatePurchase,
                IdVendor=entity.IdVendor,
                TotalWithoutTax=entity.TotalWithoutTax,
                Remise=entity.Remise,
                TotalWithoutTaxRemise=entity.TotalWithoutTaxRemise, 
                TotalTax=entity.TotalTax,
                TotalPurchase=entity.TotalPurchase,
                PaymentPurchase=entity.PaymentPurchase,
                BalancePurchase=entity.BalancePurchase,

            };
            await _appContext.Purchases.AddAsync(purchase);
            await _appContext.SaveChangesAsync();
            entity.Id = purchase.Id;

        }

        public async Task Update(PurchaseDto entity, int Id)
        {
            var purchases = await _appContext.Purchases.FindAsync(Id);
            if (purchases == null)
            {
                throw new ArgumentException("purchases not found.");
            }
            purchases.LastModificationTime = _addEntityBD.ECreationTime();
            purchases.NumberPurchase=entity.NumberPurchase;
            purchases.DatePurchase=entity.DatePurchase;
            purchases.IdVendor=entity.IdVendor;
            purchases.TotalWithoutTax=entity.TotalWithoutTax;
            purchases.Remise=entity.Remise;
            purchases.TotalWithoutTaxRemise=entity.TotalWithoutTaxRemise;
            purchases.TotalTax=entity.TotalTax;
            purchases.TotalPurchase=entity.TotalPurchase;
            purchases.PaymentPurchase=entity.PaymentPurchase;
            purchases.BalancePurchase=entity.BalancePurchase;

            await _appContext.SaveChangesAsync();

        }
    }
}
