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
    public class Product : FullAuditedEntity
    {
        [MaxLength(100)]
        public string? RefProduct { get; set; }
        [MaxLength(100)]

        [Required]
        public string Designation { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        [MaxLength(500)]
        public string? Taxe { get; set; }
        public string? BarCode { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }

        public decimal? StockQuantity { get; set; }
        public decimal? QtyAlert { get; set; }

        [ForeignKey(nameof(Unit))]
        public int? UnitId { get; set; }

        public int? Colisage { get; set; }

        [ForeignKey(nameof(Marque))]
        public int? MarqueId { get; set; }
        public Category Category { get; set; }

        public Unit Unit { get; set; }
        public Marque Marque { get; set; }
        public List<PriceLists> PriceLists { get; set; }
    }
}
