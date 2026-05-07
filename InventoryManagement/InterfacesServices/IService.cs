using InventoryManagement.Data.DTO;
using InventoryManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.InterfacesServices
{
    public interface IService<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity , int Id);
        Task DeleteAsynct(int id);
        Task<TEntity> GetAsyncById(int id);
        Task<List<TEntity>> GetAllAsyncs();
      
    }
}
