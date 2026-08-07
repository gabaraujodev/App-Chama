using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AtualizarLeituraDto
    {
        [Required]
        [Range(1, 150)]
        public int UltimoCapituloLido { get; set; }
    }
}
