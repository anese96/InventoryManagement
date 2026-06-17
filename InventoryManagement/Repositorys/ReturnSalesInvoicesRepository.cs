using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class ReturnSalesInvoicesRepository : IRepository<ReturnSalesDto>
    {
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReturnSalesDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ReturnSalesDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(ReturnSalesDto entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(ReturnSalesDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
