using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class InventoryAdjustmentViewModel
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Display(Name = "Existencia actual")]
        public float CurrentStock { get; set; }

        [Display(Name = "Tipo de movimiento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string MovementType { get; set; } = "Entrada";

        [Display(Name = "Cantidad")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public float Quantity { get; set; }

        [Display(Name = "Motivo")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Reason { get; set; } = string.Empty;
    }
}
