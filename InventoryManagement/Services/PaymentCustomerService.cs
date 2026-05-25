using InventoryManagement.Data;
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
    internal class PaymentCustomerService : IService<PaymentCustomerDto>
    {
        private readonly IRepository<PaymentCustomerDto> _repository;
        private readonly ClientRepository _repositoryClient;
        private readonly AppDbContext _appContext;

        public PaymentCustomerService(IRepository<PaymentCustomerDto> repository, ClientRepository repositoryClient, AppDbContext appContext)
        {
            _repository = repository;
            _repositoryClient = repositoryClient;
            _appContext = appContext;
        }
        public async Task AddAsync(PaymentCustomerDto entity)
        {
           if( entity == null) {
                throw new Exception("The client does not exist");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentCustomerDto>> GetAllAsyncs()
        {
           return await _repository.GetAll();
        }

        public Task<PaymentCustomerDto> GetAsyncById(int id)
        {
           if (id <= 0)
            {
                throw new ArgumentException("Invalid client ID.");
            }
            return _repository.GetById(id);
        }

        public Task UpdateAsync(PaymentCustomerDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
