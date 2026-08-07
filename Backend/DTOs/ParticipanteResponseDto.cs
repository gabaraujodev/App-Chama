namespace Backend.DTOs
{
    public class ParticipanteResponseDto
    {
        
            public int UsuarioId { get; set; }

            public string Nome { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public bool Lider { get; set; }
        
    }
}
