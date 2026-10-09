using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public String Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        [JsonIgnore]
        public ICollection<CategoryTranslation> Translations { get; set; }

        /// <summary>
        /// Returns the category name translated to the current UI culture if
        /// a translation exists in <see cref="Translations"/>; otherwise
        /// falls back to the base (Spanish) <see cref="Name"/>.
        /// </summary>
        public string GetLocalizedName()
        {
            string languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            CategoryTranslation translation = Translations?.FirstOrDefault(t => t.LanguageCode == languageCode);
            return !string.IsNullOrWhiteSpace(translation?.Name) ? translation.Name : Name;
        }
    }
}
