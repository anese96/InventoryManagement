using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class ReturnPurchaseService : IService<ReturnPurchaseDto>
    {
        private readonly IRepository<ReturnPurchaseDto> _repository;

        public ReturnPurchaseService(IRepository<ReturnPurchaseDto> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(ReturnPurchaseDto entity)
        {
            if (entity.NumberReturn == null || entity.NumberReturn == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReturnPurchaseDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public async Task<ReturnPurchaseDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public Task UpdateAsync(ReturnPurchaseDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}