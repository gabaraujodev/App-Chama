using Backend.DTOs;

namespace Backend.Services;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CriarUsuario(CriarUsuarioDto dto);

    Task<List<UsuarioResponseDto>> ListarUsuarios();
    Task<UsuarioResponseDto?> BuscarPorId(int id);
    Task<bool> DeletarPorId(int id);
    Task<UsuarioResponseDto?> UpdatePorId(AtualizarUsuarioDto dto);
}