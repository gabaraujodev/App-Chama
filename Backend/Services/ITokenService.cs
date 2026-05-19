using Backend.DTOs;

namespace Backend.Services
{
    public interface ITokenService
    {
        string Generate(UsuarioResponseDto dto);
    }
}
