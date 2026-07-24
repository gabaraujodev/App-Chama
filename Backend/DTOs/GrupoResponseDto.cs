using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class GrupoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        
    }
}
