using Shopping.Data.Entities;
using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class CustomerDetailsViewModel
    {
        public User User { get; set; }

        public List<Address> Addresses { get; set; } = new();

        public List<Sale> Orders { get; set; } = new();

        public decimal TotalSpent { get; set; }
    }

    public class ChangeOrderStatusViewModel
    {
        public int SaleId { get; set; }

        [Display(Name = "Nuevo estado")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Número de guía")]
        [MaxLength(50)]
        public string? TrackingNumber { get; set; }

        [Display(Name = "Transportadora")]
        [MaxLength(50)]
        public string? Carrier { get; set; }

        [Display(Name = "Entrega estimada")]
        [DataType(DataType.Date)]
        public DateTime? EstimatedDeliveryDate { get; set; }

        [Display(Name = "Comentarios")]
        [MaxLength(300)]
        public string? Notes { get; set; }
    }
}
