using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class VendorService : IService<VendorDto>
    {
        private readonly IRepository<VendorDto> _repository;
        private readonly VendorRepository _vendorRepository;

        public VendorService(IRepository<VendorDto> repository, VendorRepository vendorRepository)
        {
            _repository = repository;
            _vendorRepository = vendorRepository;
        }
        public async Task AddAsync(VendorDto entity)
        {
            if (entity.Name == null || entity.Name == "")
            {
                throw new ArgumentException("Un Champ Vide.");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<VendorDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public async Task<VendorDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(VendorDto entity, int Id)
        {
            if (entity.Name == null || entity.Name == "")
            {
                throw new ArgumentException("Un Champ Vide.");
            }
            await _repository.Update(entity, Id);
        }

        public async Task UpdateBalanceAsync(int id, decimal balance)
        {
            await _vendorRepository.UpdateBalance(id, balance);
        }

        public async Task UpdateTurnoverAsync(int id, decimal turnover)
        {
            await _vendorRepository.UpdateTurnover(id, turnover);
        }
    }
}
