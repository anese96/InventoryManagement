using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class VendorDto
    {
        public string RefVendor { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public decimal? Balance { get; set; } // Solde
        public decimal? Turnover { get; set; } /// Chiffre d'affaire
    }
}
