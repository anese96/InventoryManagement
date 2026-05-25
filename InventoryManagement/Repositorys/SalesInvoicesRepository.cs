using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using InventoryManagement.Data.Models;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class SalesInvoicesRepository : IRepository<SalesInvoicesDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;


        public SalesInvoicesRepository(AppDbContext appContext, AddEntityBD addEntityBD)
        {
            _appContext = appContext;
            _addEntityBD = addEntityBD;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SalesInvoicesDto>> GetAll()
        {
           return await Task.FromResult(_appContext.SalesInvoices.Select(s => new SalesInvoicesDto
            {
                Id = s.Id,
                NumberInvoice = s.NumberInvoice,
                DateInvoice = s.DateInvoice,
                IdCustomer = s.IdCustomer,
                TotalWithoutTax = s.TotalWithoutTax,
                Remise = s.Remise,
                TotalWithoutTaxRemise = s.TotalWithoutTaxRemise,
                TotalTax = s.TotalTax,
                TotalInvoice = s.TotalInvoice,
                PaymentInvoice = s.PaymentInvoice,
                BalanceInvoice = s.BalanceInvoice,
                IdCrates = s.IdCrates
            }).ToList());
        }

        public async Task<SalesInvoicesDto> GetById(int Id)
        {
            var salesInvoicesDto = await _appContext.SalesInvoices.FindAsync(Id);
            if (salesInvoicesDto == null)
            {
                throw new ArgumentException("Facture not found.");
            }
            return new SalesInvoicesDto
            {
                NumberInvoice = salesInvoicesDto.NumberInvoice,
                DateInvoice = salesInvoicesDto.DateInvoice,
                IdCustomer = salesInvoicesDto.IdCustomer,
                TotalWithoutTax = salesInvoicesDto.TotalWithoutTax,
                Remise = salesInvoicesDto.Remise,
                TotalWithoutTaxRemise = salesInvoicesDto.TotalWithoutTaxRemise,
                TotalTax = salesInvoicesDto.TotalTax,
                TotalInvoice = salesInvoicesDto.TotalInvoice,
                PaymentInvoice = salesInvoicesDto.PaymentInvoice,
                BalanceInvoice = salesInvoicesDto.BalanceInvoice,
                IdCrates = salesInvoicesDto.IdCrates

            };
        }

        public async Task Insert(SalesInvoicesDto entity)
        {
            var salesInvoices = new SalesInvoices
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                NumberInvoice=entity.NumberInvoice,
                DateInvoice=entity.DateInvoice,
                IdCustomer =entity.IdCustomer,
                TotalWithoutTax =entity.TotalWithoutTax,
                Remise =entity.Remise,
                TotalWithoutTaxRemise =entity.TotalWithoutTaxRemise,
                TotalTax =entity.TotalTax,
                TotalInvoice =entity.TotalInvoice,
                PaymentInvoice =entity.PaymentInvoice,
                BalanceInvoice =entity.BalanceInvoice,
                IdCrates =entity.IdCrates   

            };
            await _appContext.SalesInvoices.AddAsync(salesInvoices);
            await _appContext.SaveChangesAsync();
            entity.Id = salesInvoices.Id;
        }

        public async Task Update(SalesInvoicesDto entity, int Id)
        {
            var salesInvoices = await _appContext.SalesInvoices.FindAsync(Id);
            if (salesInvoices == null)
            {
                throw new ArgumentException("Facture not found.");
            }
            salesInvoices.LastModificationTime = _addEntityBD.ECreationTime();
            salesInvoices.NumberInvoice = entity.NumberInvoice;
            salesInvoices.DateInvoice = entity.DateInvoice;
            salesInvoices.IdCustomer = entity.IdCustomer;
            salesInvoices.TotalWithoutTax = entity.TotalWithoutTax;
            salesInvoices.Remise = entity.Remise;
            salesInvoices.TotalWithoutTaxRemise = entity.TotalWithoutTaxRemise;
            salesInvoices.TotalTax = entity.TotalTax;
            salesInvoices.TotalInvoice = entity.TotalInvoice;
            salesInvoices.PaymentInvoice = entity.PaymentInvoice;
            salesInvoices.BalanceInvoice = entity.BalanceInvoice;
            salesInvoices.IdCrates = entity.IdCrates;
    
            await _appContext.SaveChangesAsync();
        }
    }
}
