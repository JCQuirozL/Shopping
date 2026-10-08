using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class PaymentViewModel
    {
        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PaymentMethod { get; set; } = "CreditCard";

        [Display(Name = "Nombre del titular")]
        [MaxLength(100)]
        public string? CardHolder { get; set; }

        [Display(Name = "Número de tarjeta")]
        [MaxLength(25)]
        public string? CardNumber { get; set; }

        [Display(Name = "Vencimiento (MM/AA)")]
        [MaxLength(10)]
        public string? ExpirationDate { get; set; }

        [Display(Name = "CVV")]
        [MaxLength(4)]
        public string? Cvv { get; set; }

        public float Items { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Nombre del destinatario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingRecipient { get; set; } = string.Empty;

        [Display(Name = "Teléfono de contacto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingPhone { get; set; } = string.Empty;

        [Display(Name = "Dirección de envío")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
