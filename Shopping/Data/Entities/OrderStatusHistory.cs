using Shopping.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class OrderStatusHistory
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Sale Sale { get; set; }

        [Display(Name = "Estado")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Comentarios")]
        [MaxLength(300)]
        public string? Notes { get; set; }
    }
}
