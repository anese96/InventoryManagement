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
    public class ClientService : IService<ClientDto>
    {
        private readonly IRepository<ClientDto> _repository;

        public ClientService(IRepository<ClientDto> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(ClientDto entity)
        {
           if(entity.Name == null || entity.Name=="")
            {
                throw new ArgumentException("Le nom du client est vide.");
            }
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
    }
}
