using InventoryManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Models
{
    public class Vendor : FullAuditedEntity
    {
        [MaxLength(50)]
        public string RefVendor { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(1000)]
        public string Remark { get; set; }

        public decimal? Balance { get; set; } = 0; // Solde
        public decimal? Turnover { get; set; } = 0;/// Chiffre d'affaire

        public List<Purchase> Purchases { get; set; }
    }
}
