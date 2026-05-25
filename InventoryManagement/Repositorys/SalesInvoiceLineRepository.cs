using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using Microsoft.EntityFrameworkCore;
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
        public async Task Delete(int id)
        {
            var salesInvoiceLines = await _appContext.salesInvoiceLines.Where(sil=>sil.IdSalesInvoice==id).FirstOrDefaultAsync();
            if (salesInvoiceLines == null)
            {
                throw new ArgumentException("salesInvoiceLines not found.");
            }
            _appContext.salesInvoiceLines.Remove(salesInvoiceLines);
            await _appContext.SaveChangesAsync();
        }

        public async Task<List<SalesInvoiceLineDto>> GetAll()
        {
            return await _appContext.salesInvoiceLines
      .Select(salesInvoiceLine => new SalesInvoiceLineDto
      {
          IdSalesInvoice = salesInvoiceLine.IdSalesInvoice,
          IdProduct = salesInvoiceLine.IdProduct,
          RefProduct = salesInvoiceLine.RefProduct,
          Designation = salesInvoiceLine.Designation,
          Quantity = salesInvoiceLine.Quantity,
          Price = salesInvoiceLine.Price,
          Taxe = salesInvoiceLine.Taxe,
          TotalWithoutTax = salesInvoiceLine.TotalWithoutTax,
      })
      .ToListAsync();
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
                IdProduct = salesInvoiceLines.IdProduct,
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
           var purchasePrice = _appContext.Products.Where(p => p.Id == entity.IdProduct).
                Select(p => p.PurchasePrice).FirstOrDefault();
            var salesInvoiceLine = new SalesInvoiceLine
            {
                
                IdSalesInvoice  = entity.IdSalesInvoice,
                IdProduct = entity.IdProduct,
                RefProduct = entity.RefProduct,
                Designation = entity.Designation,
                Quantity = entity.Quantity,
                Price = entity.Price,
                PurchasePrice = (decimal)purchasePrice,
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
