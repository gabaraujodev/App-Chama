namespace Backend.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioResponseDto Usuario { get; set; } = new();
    }
}
