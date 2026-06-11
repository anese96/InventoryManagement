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
    public class VendorRepository : IRepository<VendorDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;

        public VendorRepository(AppDbContext appDbContext, AddEntityBD addEntityBD)
        {
            _appContext = appDbContext;
            _addEntityBD = addEntityBD;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<VendorDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<VendorDto> GetById(int id)
        {
            var vendor = await _appContext.Vendors.FindAsync(id);
            if (vendor == null)
            {
                throw new ArgumentException("Fournisseurs not found.");
            }
            return new VendorDto                        
            {
                RefVendor = vendor.RefVendor,
                Name = vendor.Name,
                PhoneNumber = vendor.PhoneNumber,
                Address = vendor.Address,
                Remark = vendor.Remark,     
                Balance = vendor.Balance,
                Turnover= vendor.Turnover,
            };
        }

        public async Task Insert(VendorDto entity)
        {
            var vendor = new Vendor
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                RefVendor= entity.RefVendor,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                Remark = entity.Remark,
                Balance = 0,
                Turnover = 0,
            };
            await _appContext.Vendors.AddAsync(vendor);
            await _appContext.SaveChangesAsync();
        }

        public async Task Update(VendorDto entity, int Id)
        {
            var vendor = await _appContext.Vendors.FindAsync(Id);
            if (vendor == null)
            {
                throw new ArgumentException("Fournisseurs not found.");
            }
            vendor.LastModificationTime = _addEntityBD.ECreationTime(); 
            vendor.RefVendor = entity.RefVendor;
            vendor.Name = entity.Name;
            vendor.PhoneNumber = entity.PhoneNumber;
            vendor.Address = entity.Address;
            vendor.Remark = entity.Remark;
            await _appContext.SaveChangesAsync();
        }
        public async Task UpdateBalance(int Id, decimal balance)
        {
            var vendor = await _appContext.Vendors.FindAsync(Id);
            if (vendor == null)
            {
                throw new ArgumentException("Fournisseurs not found.");
            }
            vendor.Balance += balance;
            await _appContext.SaveChangesAsync();
        }
        public async Task UpdateTurnover(int Id, decimal turnover)
        {
            var vendor = await _appContext.Vendors.FindAsync(Id);
            if (vendor == null)
            {
                throw new ArgumentException("Fournisseurs not found.");
            }
            vendor.Turnover += turnover;
            await _appContext.SaveChangesAsync();
        }
    }
}
