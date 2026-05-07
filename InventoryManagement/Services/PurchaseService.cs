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
    public class PurchaseService : IService<PurchaseDto>
    {
        private readonly IRepository<PurchaseDto> _repository;

        public PurchaseService(IRepository<PurchaseDto> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(PurchaseDto entity)
        {
            if (entity.NumberPurchase == null || entity.NumberPurchase == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PurchaseDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public async Task<PurchaseDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(PurchaseDto entity, int Id)
        {
            if (entity.NumberPurchase == null || entity.NumberPurchase == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Update(entity, Id);
        }
    }
}
