using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class PlanoLeituraResponseDto
    {
       
        public string Livro { get; set; } = string.Empty;

       
        public int CapituloInicial { get; set; }

       
        public int CapituloFinal { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
    }
}
