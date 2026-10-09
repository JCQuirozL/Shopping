using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class ProductTranslationsViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Nombre (English)")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? NameEn { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción (English)")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? DescriptionEn { get; set; }

        [Display(Name = "Nombre (Português)")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? NamePt { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción (Português)")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? DescriptionPt { get; set; }
    }
}
