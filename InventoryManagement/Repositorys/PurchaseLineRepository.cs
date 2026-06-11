using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class PurchaseLineRepository : IRepository<PurchaseLineDto>
    {
        private readonly AppDbContext _appContext;


        public PurchaseLineRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PurchaseLineDto>> GetAll()
        {
            return await _appContext.purchaseLines
                .Select(purchaseLine => new PurchaseLineDto
                {
                    IdPurchase = purchaseLine.IdPurchase,
                    IdProduct = purchaseLine.IdProduct,
                    RefProduct = purchaseLine.RefProduct,
                    Designation = purchaseLine.Designation,
                    Quantity = purchaseLine.Quantity,
                    Price = purchaseLine.Price,
                    Taxe = purchaseLine.Taxe,
                    TotalWithoutTax = purchaseLine.TotalWithoutTax
                })
                .ToListAsync();
        }

        public async Task<PurchaseLineDto> GetById(int Id)
        {
            var purchaseLine = await _appContext.purchaseLines.FindAsync(Id);
            if (purchaseLine == null)
            {
                throw new ArgumentException("PurchaseLine not found.");
            }
            return new PurchaseLineDto
            {
                IdPurchase = purchaseLine.IdPurchase,
                IdProduct = purchaseLine.IdProduct,
                RefProduct = purchaseLine.RefProduct,
                Designation = purchaseLine.Designation,
                Quantity = purchaseLine.Quantity,
                Price = purchaseLine.Price,
                Taxe = purchaseLine.Taxe,
                TotalWithoutTax = purchaseLine.TotalWithoutTax,
            };
        }

        public async Task Insert(PurchaseLineDto entity)
        {
            var purchaseLine = new PurchaseLine
            {
                IdPurchase = entity.IdPurchase,
                IdProduct = entity.IdProduct,
                RefProduct = entity.RefProduct,
                Designation = entity.Designation,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Taxe = entity.Taxe,
                TotalWithoutTax = entity.TotalWithoutTax,
            };
            await _appContext.purchaseLines.AddAsync(purchaseLine);
            await _appContext.SaveChangesAsync();
        }

        public Task Update(PurchaseLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
