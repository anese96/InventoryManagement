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
    public class SalesInvoicesDto
    {
        public int Id { get; set; }
        public string NumberInvoice { get; set; }
        public DateTime DateInvoice { get; set; }
        public int? IdCustomer { get; set; }
        public decimal? TotalWithoutTax { get; set; }
        public decimal? Remise { get; set; }
        public decimal? TotalWithoutTaxRemise { get; set; }      
        public decimal? TotalTax { get; set; }   
        public decimal? TotalInvoice { get; set; }

        public decimal? PaymentInvoice { get; set; }  
        public decimal? BalanceInvoice { get; set; }
        public int? IdCrates { get; set; }
    }
}
