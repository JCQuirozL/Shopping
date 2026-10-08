using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Data.Entities
{
    public class ExchangeRate
    {
        public int Id { get; set; }

        [Display(Name = "Moneda")]
        [MaxLength(5)]
        [Required]
        public string CurrencyCode { get; set; } = string.Empty;

        [Display(Name = "Tasa respecto al peso mexicano (MXN)")]
        [Column(TypeName = "decimal(18,6)")]
        public decimal Rate { get; set; }

        [Display(Name = "Última actualización")]
        public DateTime LastUpdated { get; set; }
    }
}
