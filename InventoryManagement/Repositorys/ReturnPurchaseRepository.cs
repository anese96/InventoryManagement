using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class ReturnPurchaseRepository : IRepository<ReturnPurchaseDto>
    {
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReturnPurchaseDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnPurchaseDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(ReturnPurchaseDto entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(ReturnPurchaseDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
