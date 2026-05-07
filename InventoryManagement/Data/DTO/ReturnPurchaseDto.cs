using InventoryManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class ReturnPurchaseDto
    {
        public string NumberReturn { get; set; }

        public DateTime DateReturn { get; set; }

        public int? IdVendor { get; set; }

     
        public decimal? TotalPurchase { get; set; }

        public decimal? TotalReturn { get; set; }
       
        public decimal? GapTotal { get; set; } // Ecart

        public int? IdCrates { get; set; }
    }
}
