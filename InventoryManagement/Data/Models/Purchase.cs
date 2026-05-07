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
    public class Purchase : FullAuditedEntity
    {
        [MaxLength(100)]
        [Required]
        public string NumberPurchase { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePurchase { get; set; }

        [ForeignKey(nameof(Vendor))]
        public int? IdVendor { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalWithoutTax { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Remise { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalWithoutTaxRemise { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalTax { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalPurchase { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PaymentPurchase { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BalancePurchase { get; set; }

        public Vendor Vendor { get; set; }

        public List<PurchaseLine> PurchaseLines { get; set; }
    }
}
