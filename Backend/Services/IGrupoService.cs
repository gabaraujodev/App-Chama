using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface IGrupoService
    {
        Task<GrupoResponseDto> CriarGrupo(CriarGrupoDto dto, int idClaim);
        Task EntrarNoGrupo(int idGrupo, int idUsuario);

        Task SairDoGrupo(int idGrupo, int idUsuario);
        Task<List<GrupoResponseDto>> ListarGrupos();
        Task<List<GrupoResponseDto>> ListarGruposDoUsuario(int idUsuario);
        Task<GrupoDetalheResponseDto> BuscarGrupoPorId(int idGrupo);
        Task<List<ParticipanteResponseDto>> ListarParticipantes(int idGrupo, int idUser);
        Task RetirarUsuarioGrupo(int idGrupo, int idUserDelete, int idUser);

    }
}
