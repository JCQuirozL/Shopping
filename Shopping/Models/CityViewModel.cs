using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public String Name { get; set; }

        public int StateId { get; set; }
    }
}
