using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class ProductTranslation
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }

        [Display(Name = "Idioma")]
        [MaxLength(5)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LanguageCode { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; } = string.Empty;
    }
}
