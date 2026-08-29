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
    public class ClientRepository : IRepository<ClientDto>
    {
        private readonly AppDbContext _appContext;
        private readonly AddEntityBD _addEntityBD;
       
        public ClientRepository(AppDbContext appDbContext, AddEntityBD addEntityBD)
        {
            _appContext = appDbContext;
            _addEntityBD = addEntityBD;
        }
        public Task<List<ClientDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ClientDto> GetById(int Id)
        {
            var client = await _appContext.Customers.FindAsync(Id);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }
            return new ClientDto
            {
                RefCustomer = client.RefCustomer,
                Name = client.Name,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                Remark= client.Remark,
                Balance = client.Balance,
                Turnover = client.Turnover
            };
        }

        public async Task Insert(ClientDto entity)
        {
           var client = new Customer
            {
                CreationTime = _addEntityBD.ECreationTime(),
                CreatorId = _addEntityBD.ECreatorId(),
                RefCustomer = entity.RefCustomer,
                Name = entity.Name,            
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                Remark=entity.Remark,
                Balance = entity.Balance,
                Turnover = 0
           };
            await _appContext.Customers.AddAsync(client);
            await _appContext.SaveChangesAsync();
        }

        public async Task Update(ClientDto entity, int Id)
        {
            var client = await _appContext.Customers.FindAsync(Id);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }
            client.LastModificationTime = _addEntityBD.ECreationTime();
            client.RefCustomer = entity.RefCustomer;
            client.Name = entity.Name;
            client.PhoneNumber = entity.PhoneNumber;
            client.Address = entity.Address;
            client.Remark = entity.Remark;    
            await _appContext.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBalance(int Id, decimal balance)
        {
            var client = await _appContext.Customers.FindAsync(Id);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }
            client.Balance += balance;
            await _appContext.SaveChangesAsync();
        }
        public async Task UpdateTurnover(int Id, decimal turnover)
        {
            var client = await _appContext.Customers.FindAsync(Id);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }
            client.Turnover += turnover;
            await _appContext.SaveChangesAsync();
        }

    }
}
