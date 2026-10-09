using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class CategoryTranslationsViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Nombre (English)")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? NameEn { get; set; }

        [Display(Name = "Nombre (Português)")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? NamePt { get; set; }
    }
}
