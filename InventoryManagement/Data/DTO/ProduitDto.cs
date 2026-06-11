using InventoryManagement.UI;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data.DTO
{
    public class ProduitDto
    {
        public int Id { get; set; }
        public string? RefProduct { get; set; }
        public string Designation { get; set; }
        public int? CategoryId { get; set; }
        public string? Taxe { get; set; }
        public string? BarCode { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }

        public decimal? StockQuantity { get; set; }
        public decimal? QtyAlert { get; set; }
        public int? UnitId { get; set; }

        public int? Colisage { get; set; }
        public int? MarqueId { get; set; }
        public int? NatureId { get; set; }
        public bool IsFavorite { get; set; }

    }
}
