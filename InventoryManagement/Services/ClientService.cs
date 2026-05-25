using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using InventoryManagement.InterfacesServices;
using InventoryManagement.Repositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class ClientService : IService<ClientDto>
    {
        private readonly IRepository<ClientDto> _repository;
        private readonly ClientRepository _repositoryClient;
        private readonly AppDbContext _appContext;

        public ClientService(IRepository<ClientDto> repository, ClientRepository repositoryClient, AppDbContext appDbContext )
        {
            _repository = repository;
            _repositoryClient = repositoryClient;
            _appContext = appDbContext;
        }
        public async Task AddAsync(ClientDto entity)
        {
            if (entity.Name == null || entity.Name == "" || entity.RefCustomer == null || entity.RefCustomer == "")
            {
                throw new ArgumentException("Le nom du client ou la référence est vide.");
            }

            // Normalize
            var refProduct = entity.RefCustomer.ToLower();
            var name = entity.Name.ToLower();

            // Vérification RefCustomer
            bool existsRef = await _appContext.Customers
                .AnyAsync(p => p.RefCustomer.ToLower() == refProduct);

            if (existsRef)
                throw new Exception("RefCustomer existe déjà.");
            // Vérification Name
            bool existsName = await _appContext.Customers
                .AnyAsync(p => p.Name.ToLower() == name);

            if (existsName)
                throw new Exception("Nom existe déjà.");

            await _repository.Insert(entity);
        }

        public Task DeleteAsynct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ClientDto>> GetAllAsyncs()
        {
            throw new NotImplementedException();
        }

        public async Task<ClientDto> GetAsyncById(int id)
        {
            if (id == null)
                throw new ArgumentException("ID null.");

            return await _repository.GetById(id);
        }

        public async Task UpdateAsync(ClientDto entity, int Id)
        {
            if (entity.Name == null || entity.Name == "")
            {
                throw new ArgumentException("Un Champ Vide.");
            }
            await _repository.Update(entity, Id);
        }

        public async Task UpdateBalanceAsync(int id, decimal balance )
        {
            await _repositoryClient.UpdateBalance(id, balance);
        }

        public async Task UpdateTurnoverAsync(int id, decimal turnover )
        {
            await _repositoryClient.UpdateTurnover(id, turnover);
        }
    }
}
