using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AtualizarUsuarioDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
