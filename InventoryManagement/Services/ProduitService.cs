using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
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
    public class ProduitService : IService<ProduitDto>
    {
        private readonly IRepository<ProduitDto> _repository;
        private readonly AppDbContext _appContext;



        public ProduitService(IRepository<ProduitDto> repository, AppDbContext appDbContext)
        {
            _repository = repository;
            _appContext = appDbContext;

        }
        public async Task AddAsync(ProduitDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // Validation champs obligatoires
            if (string.IsNullOrWhiteSpace(entity.RefProduct) ||
                string.IsNullOrWhiteSpace(entity.Designation))
            {
                throw new ArgumentException("Les champs RefProduct et Designation sont obligatoires.");
            }

            // Validation valeurs numériques
            if (entity.SalesPrice < 0 ||
                entity.PurchasePrice < 0 ||
                entity.StockQuantity < 0 ||
                entity.QtyAlert < 0)
            {
                throw new ArgumentException("Les valeurs numériques doivent être positives.");
            }

            // Normalize
            var refProduct = entity.RefProduct.ToLower();
            var designation = entity.Designation.ToLower();

            // Vérification RefProduct
            bool existsRef = await _appContext.Products
                .AnyAsync(p => p.RefProduct.ToLower() == refProduct);

            if (existsRef)
                throw new Exception("RefProduct existe déjà.");

            // Vérification Designation
            bool existsDesignation = await _appContext.Products
                .AnyAsync(p => p.Designation.ToLower() == designation);

            if (existsDesignation)
                throw new Exception("Designation existe déjà.");

            // Vérification BarCode
            if (!string.IsNullOrWhiteSpace(entity.BarCode))
            {
                bool existsBarCode = await _appContext.Products
                    .AnyAsync(p => p.BarCode == entity.BarCode);

                if (existsBarCode)
                    throw new Exception("BarCode existe déjà.");
            }

            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProduitDto>> GetAllAsyncs()
        {
           return await _repository.GetAll();
        }

        public async Task<ProduitDto> GetAsyncById(int id)
        {
            if(id == null) 
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(ProduitDto entity, int id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // Validation champs obligatoires
            if (string.IsNullOrWhiteSpace(entity.RefProduct) ||
                string.IsNullOrWhiteSpace(entity.Designation))
            {
                throw new ArgumentException("Les champs obligatoires sont vides.");
            }

            // Validation numériques
            if (entity.PurchasePrice < 0 ||
                entity.SalesPrice < 0 ||
                entity.StockQuantity < 0 ||
                entity.QtyAlert < 0)
            {
                throw new ArgumentException("Valeurs invalides.");
            }

            // Vérification RefProduct
            bool existsRef = await _appContext.Products
                .AnyAsync(p =>
                    p.RefProduct.ToLower() == entity.RefProduct.ToLower() &&
                    p.Id != id);

            if (existsRef)
            {
                throw new Exception("RefProduct existe déjà.");
            }

            // Vérification Designation
            bool existsDesignation = await _appContext.Products
                .AnyAsync(p =>
                    p.Designation.ToLower() == entity.Designation.ToLower() &&
                    p.Id != id);

            if (existsDesignation)
            {
                throw new Exception("Designation existe déjà.");
            }

            // Vérification BarCode
            if (!string.IsNullOrWhiteSpace(entity.BarCode))
            {
                bool existsBarCode = await _appContext.Products
                    .AnyAsync(p =>
                        p.BarCode == entity.BarCode &&
                        p.Id != id);

                if (existsBarCode)
                {
                    throw new Exception("BarCode existe déjà.");
                }
            }

            await _repository.Update(entity, id);
        }


    }
}
