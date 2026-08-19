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
    public class SalesInvoices : FullAuditedEntity
    {
        [MaxLength(100)]
        [Required]
        public string NumberInvoice { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateInvoice { get; set; }

        [ForeignKey(nameof(Customer))]
        public int? IdCustomer { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalWithoutTax { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? Remise { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalWithoutTaxRemise { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalTax { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? TotalInvoice { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? PaymentInvoice { get; set; }
        [Column(TypeName = "decimal(18, 20)")]
        public decimal? BalanceInvoice { get; set; }
        [ForeignKey(nameof(Crates))]
        public int? IdCrates { get; set; }
        public Customer Customer { get; set; }
        public List<SalesInvoiceLine> salesInvoiceLines { get; set; }
        public Crates Crates { get; set; }
    }
}
