using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class PurchaseLineDto
    {
        public int IdPurchase { get; set; }    
        public string RefProduct { get; set; }
        public string Designation { get; set; }     
        public decimal Quantity { get; set; }   
        public decimal Price { get; set; }
        public string Taxe { get; set; }  
        public decimal TotalWithoutTax { get; set; }
    }
}
