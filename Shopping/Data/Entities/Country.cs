using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public String Name { get; set; }

        public ICollection<State> States{ get; set; }

        [Display(Name = "Estados / Departamentos")]
        public int StatesNumber => States == null ? 0 : States.Count;
    }
}
