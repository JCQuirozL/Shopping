using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        public User User { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
        
        
        public OrderStatus OrderStatus { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<OrderStatusHistory> StatusHistories { get; set; }

        [Display(Name = "Destinatario")]
        [MaxLength(100)]
        public string? ShippingRecipient { get; set; }

        [Display(Name = "Teléfono de contacto")]
        [MaxLength(20)]
        public string? ShippingPhone { get; set; }

        [Display(Name = "Dirección de envío")]
        [MaxLength(300)]
        public string? ShippingAddress { get; set; }

        [Display(Name = "Número de guía")]
        [MaxLength(50)]
        public string? TrackingNumber { get; set; }

        [Display(Name = "Transportadora")]
        [MaxLength(50)]
        public string? Carrier { get; set; }

        [Display(Name = "Entrega estimada")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? EstimatedDeliveryDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => SaleDetails == null ? 0 : SaleDetails.Count;
        
        
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => SaleDetails == null ? 0 : SaleDetails.Sum(sd => sd.Quantity);
        
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => SaleDetails == null ? 0 : SaleDetails.Sum(sd => sd.Value);
    }
}
