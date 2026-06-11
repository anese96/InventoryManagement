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
    public class PaymentVendorService : IService<PaymentVendorDto>
    {
        private readonly IRepository<PaymentVendorDto> _repository;
      
        private readonly AppDbContext _appContext;

        public PaymentVendorService(IRepository<PaymentVendorDto> repository, AppDbContext appContext)
        {
            _repository = repository;
       
            _appContext = appContext;
        }
        public async Task AddAsync(PaymentVendorDto entity)
        {
            if (entity == null)
            {
                throw new Exception("The payment does not exist");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentVendorDto>> GetAllAsyncs()
        {
            return await _repository.GetAll();
        }

        public Task<PaymentVendorDto> GetAsyncById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid client ID.");
            }
            return _repository.GetById(id);
        }

        public Task UpdateAsync(PaymentVendorDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
