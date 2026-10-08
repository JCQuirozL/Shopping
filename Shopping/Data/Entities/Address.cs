using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [Display(Name = "Nombre del destinatario")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Recipient { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Calle y número")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Street { get; set; } = string.Empty;

        [Display(Name = "Colonia / Barrio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? Neighborhood { get; set; }

        [Display(Name = "Código postal")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "Referencias")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? References { get; set; }

        public City City { get; set; }

        [Display(Name = "Predeterminada")]
        public bool IsDefault { get; set; }

        [Display(Name = "Dirección completa")]
        public string FullAddress => $"{Street}" +
            (string.IsNullOrWhiteSpace(Neighborhood) ? string.Empty : $", {Neighborhood}") +
            $", {City?.Name}, CP {PostalCode}";
    }
}
