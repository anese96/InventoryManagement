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
    public class SalesInvoicesService : IService<SalesInvoicesDto>
    {
        private readonly IRepository<SalesInvoicesDto> _repository;

        public SalesInvoicesService(IRepository<SalesInvoicesDto> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(SalesInvoicesDto entity)
        {
            if (entity.NumberInvoice == null || entity.NumberInvoice == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesInvoicesDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public async Task<SalesInvoicesDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(SalesInvoicesDto entity, int Id)
        {
            if (entity.NumberInvoice == null || entity.NumberInvoice == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Update(entity, Id);
        }
    }
}
