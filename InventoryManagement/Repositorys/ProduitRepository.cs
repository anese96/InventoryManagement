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

namespace InventoryManagement.Repositorys
{
    public class ProduitRepository : IRepository<ProduitDto>
    {

        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public ProduitRepository(AppDbContext appDbContext, AddEntityBD addEntityBD)
        {
            _appContext = appDbContext;
            _addEntityBD = addEntityBD;
        }

        public async Task ModifierQty(int id, int qty , bool isAddition)
        {
            var produit = await _appContext.Products.FindAsync(id);
            if (produit == null)
            {
                throw new ArgumentException("Produit not found.");
            }
           if (isAddition)
            {
                produit.StockQuantity += qty;
            }
            else
            {
                //if (produit.StockQuantity < qty)
                //{
                //    throw new Exception($"La quantité est insuffisante pour le produit: {produit.Designation}");
                //}
                produit.StockQuantity -= qty;
            }
            await _appContext.SaveChangesAsync();
        }

        public async Task Insert(ProduitDto entity)
        {
            var produit = new Product
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                RefProduct = entity.RefProduct,
                Designation = entity.Designation,
                CategoryId = entity.CategoryId,
                Taxe = entity.Taxe,
                BarCode = entity.BarCode,
                PurchasePrice = entity.PurchasePrice,
                SalesPrice = entity.SalesPrice,
                StockQuantity = entity.StockQuantity,
                QtyAlert = entity.QtyAlert,
                UnitId = entity.UnitId,
                Colisage = entity.Colisage,
                MarqueId = entity.MarqueId,
                NatureId = entity.NatureId,
                IsFavorite= entity.IsFavorite

            };
            await _appContext.Products.AddAsync(produit);
            await _appContext.SaveChangesAsync();
            entity.Id = produit.Id;
          
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProduitDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ProduitDto> GetById(int Id)
        {
            var produit = await _appContext.Products.FindAsync(Id);
            if (produit == null)
            {
                throw new ArgumentException("Produit not found.");
            }
            return new ProduitDto
            {
                RefProduct = produit.RefProduct,
                Designation = produit.Designation,
                CategoryId = produit.CategoryId,
                Taxe = produit.Taxe,
                BarCode = produit.BarCode,
                PurchasePrice = produit.PurchasePrice,
                SalesPrice = produit.SalesPrice,
                StockQuantity = produit.StockQuantity,
                QtyAlert = produit.QtyAlert,
                UnitId = produit.UnitId,
                Colisage = produit.Colisage,
                MarqueId = produit.MarqueId,
                NatureId = produit.NatureId,
                IsFavorite = produit.IsFavorite
            };
        }
        public async Task Update(ProduitDto entity, int Id)
        {
            var produit = await _appContext.Products.FindAsync(Id);
            if (produit == null)
            {
                throw new ArgumentException("Produit not found.");
            }
            produit.LastModificationTime = _addEntityBD.ECreationTime();
            produit.RefProduct = entity.RefProduct;
            produit.Designation = entity.Designation;
            produit.BarCode = entity.BarCode;
            produit.CategoryId = entity.CategoryId;
            produit.Colisage = entity.Colisage;
            produit.MarqueId = entity.MarqueId;
            produit.NatureId = entity.NatureId;
            produit.PurchasePrice = entity.PurchasePrice;
            produit.SalesPrice = entity.SalesPrice;
            produit.StockQuantity = entity.StockQuantity;
            produit.QtyAlert = entity.QtyAlert;
            produit.Taxe = entity.Taxe;
            produit.UnitId = entity.UnitId;
            produit.IsFavorite = entity.IsFavorite;
            await _appContext.SaveChangesAsync();
        }


    }
}
