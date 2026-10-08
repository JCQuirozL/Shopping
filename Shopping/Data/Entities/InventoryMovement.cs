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

        [Display(Name = "Motivo")]
        [MaxLength(200)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Reason { get; set; } = string.Empty;

        [Display(Name = "Usuario")]
        public string? UserName { get; set; }
    }
}
