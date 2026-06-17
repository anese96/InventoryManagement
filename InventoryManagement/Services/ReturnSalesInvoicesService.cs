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
    public class ReturnSalesInvoicesService : IService<ReturnSalesDto>
    {
        private readonly IRepository<ReturnSalesDto> _repository;

        public ReturnSalesInvoicesService(IRepository<ReturnSalesDto> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(ReturnSalesDto entity)
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

        public async Task<List<ReturnSalesDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public async Task<ReturnSalesDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(ReturnSalesDto entity, int Id)
        {
            if (entity.NumberReturn == null || entity.NumberReturn == "")
            {
                throw new ArgumentException("Le Numéro de facture est vide.");
            }
            await _repository.Update(entity, Id);
        }
    }
}
