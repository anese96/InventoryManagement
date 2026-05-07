using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class ReturnSalesDto
    {
        public string NumberReturn { get; set; }
        public DateTime DateReturn { get; set; }
        public int? IdCustomer { get; set; }
        public decimal? TotalInvoice { get; set; }
        public decimal? TotalReturn { get; set; }
        public decimal? GapTotal { get; set; } // Ecart
        public int? IdCrates { get; set; }

    }
}
