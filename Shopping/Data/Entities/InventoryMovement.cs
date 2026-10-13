using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class InventoryMovement
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float QuantityChange { get; set; }

        [Display(Name = "Existencia resultante")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float ResultingStock { get; set; }

        [Display(Name = "Documento soporte")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public InventoryDocumentType DocumentType { get; set; }

        [Display(Name = "Número de documento")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Display(Name = "Motivo / Comentarios")]
        [MaxLength(200)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Reason { get; set; } = string.Empty;

        [Display(Name = "Usuario")]
        public string? UserName { get; set; }
    }
}
