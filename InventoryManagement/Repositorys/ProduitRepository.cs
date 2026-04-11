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

        public ProduitRepository(AppDbContext appDbContext , AddEntityBD addEntityBD )
        {
            _appContext=appDbContext;
            _addEntityBD = addEntityBD;
        }

        public Task Insert(ProduitDto entity)
        {
           var produit = new Product
            {
               CreationTime = _addEntityBD.ECreationTime(),
               CreatorId = _addEntityBD.ECreatorId(),

               RefProduct = entity.RefProduct,
               Designation = entity.Designation,
                CategoryId= entity.CategoryId,          
               Taxe = entity.Taxe,
               BarCode = entity.BarCode,
               PurchasePrice = entity.PurchasePrice,
               SalesPrice = entity.SalesPrice,
               StockQuantity = entity.StockQuantity,
               QtyAlert = entity.QtyAlert,
               UnitId = entity.UnitId,
               Colisage = entity.Colisage,
               MarqueId = entity.MarqueId

           };
            _appContext.Products.Add(produit);
            return _appContext.SaveChangesAsync();
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProduitDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProduitDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
        public Task Update(ProduitDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
