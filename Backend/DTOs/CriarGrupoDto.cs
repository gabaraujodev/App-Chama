using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CriarGrupoDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 3)]

        public string Descricao { get; set; } = string.Empty;


    }
}
