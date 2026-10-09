using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [Display(Name = "Idioma")]
        [MaxLength(5)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LanguageCode { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = string.Empty;
    }
}
