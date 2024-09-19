using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;      
    }
}
