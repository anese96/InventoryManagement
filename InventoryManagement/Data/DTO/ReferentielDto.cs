using InventoryManagement.UI;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Data.DTO
{
    /// <summary>
    /// Shared DTO for simple name-only referentiels: Catégorie, Unité, Nature, Marque.
    /// </summary>
    public class ReferentielDto
    {
        [FormControl(FormControlType.ReadOnly)]
        [FormLabel("Id")]
        public int Id { get; set; }

        [Required]
        [FormLabel("Nom")]
        public string Name { get; set; }
    }
}
