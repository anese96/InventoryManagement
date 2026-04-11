using InventoryManagement.UI;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data.DTO
{
    public class ClientDto
    {
        [FormSkip]
        public int? Id { get; set; }

        [Required]
        [FormLabel("Nom")]
        [FormLayout(Column = 0, Order = 0)]
        public string Nom { get; set; }

        [FormLabel("Prénom")]
        [FormLayout(Column = 0, Order = 1)]
        public string? Prenom { get; set; }

        [FormLabel("Téléphone")]
        [FormLayout(Column = 0, Order = 2)]
        public string? Telephone { get; set; }

        [FormLabel("Email")]
        [FormLayout(Column = 0, Order = 3)]
        public string? Email { get; set; }

        [FormLabel("Adresse")]
        [FormLayout(Column = 1, Order = 0)]
        public string? Adresse { get; set; }

        [FormLabel("Ville")]
        [FormLayout(Column = 1, Order = 1)]
        public string? Ville { get; set; }

        [FormLabel("Code Postal")]
        [FormLayout(Column = 1, Order = 2)]
        public string? CodePostal { get; set; }

        [FormLabel("Plafond Crédit")]
        [FormControl(FormControlType.DecimalTextBox)]
        [FormLayout(Column = 1, Order = 3)]
        public decimal? PlafondCredit { get; set; }

        [FormLabel("Actif")]
        [FormControl(FormControlType.CheckBox)]
        [FormLayout(Column = 1, Order = 4)]
        public bool Actif { get; set; } = true;

        [FormLabel("Notes")]
        [FormLayout(Column = 1, Order = 5)]
        public string? Notes { get; set; }
    }
}
