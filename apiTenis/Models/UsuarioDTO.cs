using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class UsuarioDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
