using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginUsuarioDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]

        public string Senha { get; set; } = string.Empty;
    }
}
