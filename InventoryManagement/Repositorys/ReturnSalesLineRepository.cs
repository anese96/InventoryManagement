using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class ReturnSalesLineRepository : IRepository<ReturnSalesLineDto>
    {
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReturnSalesLineDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnSalesLineDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(ReturnSalesLineDto entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(ReturnSalesLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}