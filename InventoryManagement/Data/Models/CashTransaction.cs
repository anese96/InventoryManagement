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
    public class CashTransaction : FullAuditedEntity
    {
        [ForeignKey(nameof(Crates))]
        public int? IdCrates { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateCash { get; set; }
        public int? Type { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CashAmount { get; set; } //Montant

        public string Subject { get; set; } // Source
        
        public Crates Crates { get; set; }
    }
}
