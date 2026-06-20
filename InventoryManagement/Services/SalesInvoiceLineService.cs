using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class SalesInvoiceLineService : IService<SalesInvoiceLineDto>
    {
        private readonly IRepository<SalesInvoiceLineDto> _repository;
        private readonly AppDbContext _appContext;

        public SalesInvoiceLineService(IRepository<SalesInvoiceLineDto> repository,  AppDbContext  appDbContext)
        {
            _repository=repository;
            _appContext = appDbContext;
        }
        public async Task AddAsync(SalesInvoiceLineDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.IdSalesInvoice <= 0 ||
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
            if (product.StockQuantity < entity.Quantity)
            {
                 throw new Exception($"Quantité insuffisante en stock pour le produit : {entity.Designation}");

            }

            await _repository.Insert(entity);
        }

        public async Task DeleteAsynct(int id)
        {
             await _repository.Delete(id);
        }

        public async Task<List<SalesInvoiceLineDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public async Task<SalesInvoiceLineDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public Task UpdateAsync(SalesInvoiceLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
