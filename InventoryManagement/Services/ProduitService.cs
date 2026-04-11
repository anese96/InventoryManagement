using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
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
        public Task AddAsync(ProduitDto entity)
        {
            // Validate required fields
            if ( entity.RefProduct==""|| entity.Designation == "" || entity.Taxe=="" || entity.SalesPrice == null || entity.SalesPrice < 0 || entity.StockQuantity==null)
            {
                return Task.FromException(new ArgumentException("Un Champ Vide."));
            }
         
            return _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProduitDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public Task<ProduitDto> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ProduitDto entity , int Id )
        {
            // Validate required fields
            if (entity.RefProduct == null || entity.Designation == null || entity.Taxe == null || entity.SalesPrice == null || entity.SalesPrice < 0 || entity.StockQuantity == null)
            {
                return Task.FromException(new ArgumentException("Un Champ Vide."));
            }

            return _repository.Update(entity, Id);
        }
    }
}
