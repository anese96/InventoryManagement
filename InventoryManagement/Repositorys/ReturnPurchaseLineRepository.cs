using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class ReturnPurchaseLineRepository : IRepository<ReturnPurchaseLineDto>
    {
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReturnPurchaseLineDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnPurchaseLineDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(ReturnPurchaseLineDto entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(ReturnPurchaseLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}