using Backend.DTOs;

namespace Backend.Services;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CriarUsuario(CriarUsuarioDto dto);

    Task<UsuarioResponseDto> BuscarPorId(int id);
    Task DeletarPorId(int id, int idUser);
    Task<UsuarioResponseDto> UpdatePorId(int id, int idUser, AtualizarUsuarioDto dto);
    Task<UsuarioResponseDto> Login(LoginUsuarioDto dto);
   
}