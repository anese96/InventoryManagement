using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Models
{
    public class ReturnPurchaseLine :Entity.Entity
    {
        [ForeignKey(nameof(ReturnPurchase))]
        [Required]
        public int IdReturnSales { get; set; }

        [ForeignKey(nameof(Product))]
        [Required]
        public int IdProduct { get; set; }
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

        public ReturnPurchase ReturnPurchase { get; set; }
        public Product Product { get; set; }
    }
}
