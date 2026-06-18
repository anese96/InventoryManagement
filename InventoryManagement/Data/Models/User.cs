using Accessibility;
using InventoryManagement.Data.Entity;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }= true;
    }
}
