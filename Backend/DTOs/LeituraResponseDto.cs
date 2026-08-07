namespace Backend.DTOs
{
    public class LeituraResponseDto
    {
        public string Livro { get; set; } = string.Empty;


        public int CapituloInicial { get; set; }


        public int CapituloFinal { get; set; }
        public int UltimoCapituloLido { get; set; }
        public int Percentual { get; set; } 
    }
}
