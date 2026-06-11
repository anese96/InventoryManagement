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
    public class PurchaseDto
    {
        public int Id { get; set; }
        public string NumberPurchase { get; set; }

       
        public DateTime DatePurchase { get; set; }

       
        public int? IdVendor { get; set; }
      
        public decimal? TotalWithoutTax { get; set; }
       
        public decimal? Remise { get; set; }
       
        public decimal? TotalWithoutTaxRemise { get; set; }

        public decimal? TotalTax { get; set; }

        public decimal? TotalPurchase { get; set; }
   
        public decimal? PaymentPurchase { get; set; }
   
        public decimal? BalancePurchase { get; set; }
        public int? IdCrates { get; set; }
    }
}
