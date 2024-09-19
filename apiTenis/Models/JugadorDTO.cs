using apiTenis.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    [RangosParaJugadorAttributes]
    public class JugadorDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;
        [Required(ErrorMessage = "El género es obligatorio")]
        [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser 'M' (Masculino) o 'F' (Femenino)")]
        public string Genero { get; set; } = string.Empty;
        [Required(ErrorMessage = "El nivel es obligatorio")]
        [Range(1, 100, ErrorMessage = "El nivel debe estar entre 1 y 100")]
        public int Nivel { get; set; }
        [Range(1, 100, ErrorMessage = "La Fuerza debe estar entre 1 y 100")]
        public int? Fuerza { get; set; }
        [Range(1, 100, ErrorMessage = "La Velocidad debe estar entre 1 y 100")]
        public int? Velocidad { get; set; }
        [Range(1, 100, ErrorMessage = "El Tiempo de reaccion debe estar entre 1 y 100")]
        public int? TiempoReaccion { get; set; }

    }
}
