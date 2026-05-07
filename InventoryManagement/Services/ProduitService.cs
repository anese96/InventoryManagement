using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
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
       

        public ProduitService(IRepository<ProduitDto> repository )
        {
            _repository = repository;
           
        }
        public async  Task AddAsync(ProduitDto entity)
        {
            // Validate required fields
            if (entity.RefProduct == null || entity.Designation == null || entity.Taxe == null || entity.SalesPrice == null || entity.SalesPrice < 0 || entity.PurchasePrice == null || entity.PurchasePrice < 0 || entity.StockQuantity == null || entity.StockQuantity < 0 || entity.QtyAlert < 0)
            {
                throw new ArgumentException("Un Champ Vide.");
            }
         
            await  _repository.Insert(entity);
           
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

        public async Task UpdateAsync(ProduitDto entity , int Id )
        {
            // Validate required fields
            if (entity.RefProduct == null || entity.Designation == null || entity.Taxe == null || entity.SalesPrice == null || entity.SalesPrice < 0 || entity.PurchasePrice == null || entity.PurchasePrice < 0 || entity.StockQuantity == null || entity.StockQuantity < 0 || entity.QtyAlert < 0)
            {
                throw new ArgumentException("Un Champ Vide.");
            }

            await _repository.Update(entity, Id);
        }


    }
}
