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
    public class PaymentCustomerDto
    {
        public int IdCustomer { get; set; }
        public int IdCrates { get; set; }
        public string NumberPayment { get; set; }
        public DateTime DatePayment { get; set; }

        public decimal? Payment { get; set; } //  Montant

    }
}
