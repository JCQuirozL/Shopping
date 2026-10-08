using Shopping.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
        
        public ICollection<TemporalSale> TemporalSales { get; set; }

        [Display(Name = "Nombre del destinatario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingRecipient { get; set; } = string.Empty;

        [Display(Name = "Teléfono de contacto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingPhone { get; set; } = string.Empty;

        [Display(Name = "Dirección de envío")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ShippingAddress { get; set; } = string.Empty;

        public int? SelectedAddressId { get; set; }

        public List<Address> SavedAddresses { get; set; } = new();

        
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Quantity);
        
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Value);
    }
}

