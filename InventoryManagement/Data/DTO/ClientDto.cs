using InventoryManagement.UI;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data.DTO
{
    public class ClientDto
    {
        public string RefCustomer { get; set; }      
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public decimal? Balance { get; set; } // Solde
        public decimal? Turnover { get; set; } /// Chiffre d'affaire
    }
}
