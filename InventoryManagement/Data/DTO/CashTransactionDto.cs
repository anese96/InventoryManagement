using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class CashTransactionDto
    {
        public int? IdCrates { get; set; }    
        public DateTime? DateCash { get; set; }
        public int? Type { get; set; }
        public decimal? CashAmount { get; set; } //Montant
        public string Subject { get; set; } // Source
    }
}
