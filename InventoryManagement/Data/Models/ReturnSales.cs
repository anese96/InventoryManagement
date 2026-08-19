using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Models
{
    public class ReturnSales :Entity.Entity
    {
        [MaxLength(100)]
        [Required]
        public string NumberReturn { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateReturn { get; set; }

        [ForeignKey(nameof(Customer))]
        public int? IdCustomer { get; set; }

        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalInvoice { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalReturn { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? GapTotal { get; set; } // Ecart

        [ForeignKey(nameof(Crates))]
        public int? IdCrates { get; set; }
        public Customer Customer { get; set; }
        public Crates Crates { get; set; }
        public List<ReturnSalesLine> ReturnSalesLine { get; set; }
    }
}
