using InventoryManagement.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Data.Models
{
    public class PriceLists : BaseEntity
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }       
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? Price { get; set; }

        public Product Product { get; set; }
    }
}