using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IGrupoService
    {
        Task<GrupoResponseDto> CriarGrupo(CriarGrupoDto dto, int idClaim);
        Task<bool> GrupoExiste(int id);
        Task<bool> EntrarNoGrupo(int idGrupo, int idUsuario);

        Task<bool> SairDoGrupo(int idGrupo, int idUsuario);
        Task<List<GrupoResponseDto>> ListarGrupos();
        Task<List<GrupoResponseDto>> ListarGruposDoUsuario(int idUsuario);
        Task<GrupoDetalheResponseDto?> BuscarGrupoPorId(int idGrupo);

    }
}
