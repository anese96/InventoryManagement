using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class SalesInvoiceLineRepository : IRepository<SalesInvoiceLineDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public SalesInvoiceLineRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesInvoiceLineDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<SalesInvoiceLineDto> GetById(int Id)
        {
            var salesInvoiceLines = await _appContext.salesInvoiceLines.FindAsync(Id);
            if (salesInvoiceLines == null)
            {
                throw new ArgumentException("salesInvoiceLines not found.");
            }
            return new SalesInvoiceLineDto
            {
                IdSalesInvoice = salesInvoiceLines.IdSalesInvoice,
                RefProduct = salesInvoiceLines.RefProduct,   
                Designation = salesInvoiceLines.Designation,
                Quantity = salesInvoiceLines.Quantity,
                Price = salesInvoiceLines.Price,
                Taxe = salesInvoiceLines.Taxe,
                TotalWithoutTax = salesInvoiceLines.TotalWithoutTax,

            };

        }

        public async Task Insert(SalesInvoiceLineDto entity)
        {
            var salesInvoiceLine = new SalesInvoiceLine
            {
                
                IdSalesInvoice  = entity.IdSalesInvoice,
                RefProduct = entity.RefProduct,
                Designation = entity.Designation,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Taxe = entity.Taxe,
                TotalWithoutTax = entity.TotalWithoutTax,

            };
            await _appContext.salesInvoiceLines.AddAsync(salesInvoiceLine);
            await _appContext.SaveChangesAsync();

        }

        public async Task Update(SalesInvoiceLineDto entity, int Id)
        {
            var salesInvoiceLines = await _appContext.salesInvoiceLines.FindAsync(Id);
            if (salesInvoiceLines == null)
            {
                throw new ArgumentException("salesInvoiceLines not found.");
            }
            salesInvoiceLines.IdSalesInvoice= entity.IdSalesInvoice;
            salesInvoiceLines.RefProduct=entity.RefProduct;
            salesInvoiceLines.Designation=entity.Designation;
            salesInvoiceLines.Quantity=entity.Quantity;
            salesInvoiceLines.Price=entity.Price;
            salesInvoiceLines.Taxe=entity.Taxe;
            salesInvoiceLines.TotalWithoutTax=entity.TotalWithoutTax;


            await _appContext.SaveChangesAsync();

        }
    }
}
