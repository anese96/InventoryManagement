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
    public class SalesInvoiceLineService : IService<SalesInvoiceLineDto>
    {
        private readonly IRepository<SalesInvoiceLineDto> _repository;

        public SalesInvoiceLineService(IRepository<SalesInvoiceLineDto> repository)
        {
            _repository=repository;
        }
        public async Task AddAsync(SalesInvoiceLineDto entity)
        {
           if (entity.IdSalesInvoice == null || entity.RefProduct==null|| entity.RefProduct =="" 
                ||entity.Designation==null || entity.Designation=="" || entity.Quantity==null|| entity.Quantity<0
                || entity.Price==null||entity.Price<0 ||entity.Taxe==null ||entity.TotalWithoutTax==null||entity.TotalWithoutTax<0)
            {
                throw new ArgumentException("Un Champ Vide.");
            }
            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesInvoiceLineDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public async Task<SalesInvoiceLineDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public Task UpdateAsync(SalesInvoiceLineDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
