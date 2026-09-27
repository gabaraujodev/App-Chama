using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AtualizarPlanoLeituraDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Livro { get; set; } = string.Empty;

        [Range(1, 150)]
        public int CapituloInicial { get; set; }

        [Range(1, 150)]
        public int CapituloFinal { get; set; }

        [Required]

        public DateTime DataInicio { get; set; }
        [Required]
        public DateTime DataFim { get; set; }
    }
}
