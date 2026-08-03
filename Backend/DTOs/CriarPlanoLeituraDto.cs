using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CriarPlanoLeituraDto
    {
        [Required]
        public string Livro { get; set; } = string.Empty;

        [Required]
        public int CapituloInicial { get; set; }

        [Required]
        public int CapituloFinal { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }
    }
}
