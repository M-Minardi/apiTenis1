using apiTenis.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Entities
{
    public class Torneo
    {
        public int Id { get; set; }
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

        [Required]
        public int JugadorGanadorId { get; set; }
        public Jugador? JugadorGanador { get; set; }
    }
}
