using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
    }
}
