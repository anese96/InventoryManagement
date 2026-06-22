using DocumentFormat.OpenXml.Math;
using InventoryManagement.Data;
using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class UserRepository : IRepository<UserDto>
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public UserDto? Login(string username, string password)
        {
            var user = _appDbContext.Users
                .FirstOrDefault(x =>
                    x.UserName == username &&
                    x.IsActive);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            user.LastLogin = DateTime.Now;
            _appDbContext.SaveChanges();

            return new UserDto
            {
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                LastLogin = user.LastLogin
            };
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(UserDto entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(UserDto entity, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
