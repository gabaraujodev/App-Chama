namespace Backend.DTOs
{
    public class ParticipanteProgressoResponseDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int UltimoCapituloLido { get; set; }
        public int Percentual { get; set; }
        public bool Concluiu { get; set; }
    }
}
