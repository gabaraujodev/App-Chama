namespace Backend.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UsuarioResponseDto Usuario { get; set; }
    }
}
