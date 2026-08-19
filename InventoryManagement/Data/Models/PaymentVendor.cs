using InventoryManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Models
{
    public class PaymentVendor : FullAuditedEntity
    {
        [ForeignKey(nameof(Vendor))]
        [Required]
        public int IdVendor { get; set; }

        [Required]
        public string NumberPayment { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime DatePayment { get; set; }

        [Column(TypeName = "decimal(18, 20)")]
        [Required]
        public decimal? Payment { get; set; } //  Montant
        [ForeignKey(nameof(Crates))]
        public int IdCrates { get; set; }
        public Vendor Vendor { get; set; }
        public Crates Crates { get; set; }
    }
}
