using apiTenis.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class TorneoCrearDTO
    {        
        [Required(ErrorMessage = "El nombre del torneo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del torneo no puede tener más de 50 caracteres.")]
        [PrimeraLetraMayusculaAttributes(ErrorMessage = "La primera letra del nombre debe ser mayúscula.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es obligatorio.")]
        [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser 'M' (Masculino) o 'F' (Femenino).")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha del torneo es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha debe ser una fecha válida.")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "La lista de jugadores es obligatoria.")]
        [MinLength(2, ErrorMessage = "El torneo debe tener al menos 2 jugadores.")]
        [CantidadParAttributes(ErrorMessage = "La cantidad de jugadores debe ser un número par.")]
        public List<long> JugadoresIds { get; set; } = new List<long>();

    }
}
