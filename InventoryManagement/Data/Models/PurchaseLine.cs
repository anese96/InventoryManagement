using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Data.Models
{
    public class PurchaseLine : Entity.Entity
    {
        [ForeignKey(nameof(Purchase))]
        [Required]
        public int IdPurchase { get; set; }
        [MaxLength(100)]
        [Required]
        public string RefProduct { get; set; }
        [MaxLength(100)]

        [Required]
        public string Designation { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal Price { get; set; }

        public string Taxe { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal TotalWithoutTax { get; set; }

        public Purchase Purchase { get; set; }
    }
}