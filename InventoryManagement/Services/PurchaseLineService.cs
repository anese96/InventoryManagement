using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class PurchaseLineService : IService<PurchaseLineDto>
    {
        private readonly IRepository<PurchaseLineDto> _repository;
        private readonly AppDbContext _appContext;

        public PurchaseLineService(IRepository<PurchaseLineDto> repository, AppDbContext appContext)
        {
            _repository = repository;
            _appContext = appContext;
        }
        public async Task AddAsync(PurchaseLineDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.IdPurchase <= 0 ||
                string.IsNullOrWhiteSpace(entity.RefProduct) ||
                string.IsNullOrWhiteSpace(entity.Designation) ||
                entity.Quantity <= 0 ||
                entity.Price < 0 ||

                entity.TotalWithoutTax < 0)
            {
                throw new ArgumentException("Un ou plusieurs champs sont invalides.");
            }

            // Recherche du produit
            var product = await _appContext.Products
                .FirstOrDefaultAsync(x => x.Id == entity.IdProduct);

            if (product == null)
                throw new Exception("Produit introuvable.");
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PurchaseLineDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public async Task<PurchaseLineDto> GetAsyncById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID invalide.");

            return await _repository.GetById(id);
        }

        public Task UpdateAsync(PurchaseLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
