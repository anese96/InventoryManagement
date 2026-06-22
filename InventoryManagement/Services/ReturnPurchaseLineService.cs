using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
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
    public class ReturnPurchaseLineService : IService<ReturnPurchaseLineDto>
    {
        private readonly IRepository<ReturnPurchaseLineDto> _repository;
        private readonly AppDbContext _appContext;

        public ReturnPurchaseLineService(IRepository<ReturnPurchaseLineDto> repository, AppDbContext appContext)
        {
            _repository = repository;
            _appContext = appContext;
        }
        public async Task AddAsync(ReturnPurchaseLineDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.IdReturnPurchase <= 0 ||
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

            // Vérification du stock
            //if (product.StockQuantity < entity.Quantity)
            //{
            //    throw new Exception("Quantité insuffisante en stock.");
            //}

            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReturnPurchaseLineDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public async Task<ReturnPurchaseLineDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public Task UpdateAsync(ReturnPurchaseLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
