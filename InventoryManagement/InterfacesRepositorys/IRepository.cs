using InventoryManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.InterfacesRepositorys
{
    public interface IRepository <TEntity> where TEntity : class
    {
        Task Insert(TEntity entity);
        Task Update(TEntity entity, int Id);
        Task Delete(int id);
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
    }
}
