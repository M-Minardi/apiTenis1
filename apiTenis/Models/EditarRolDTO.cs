using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class EditarRolDTO
    {
        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        [Required]
        public string NombreRol { get; set; } = string.Empty;
    }
}
