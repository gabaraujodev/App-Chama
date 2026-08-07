using Backend.DTOs;

namespace Backend.Services
{
    public interface ILeituraService
    {
        Task MarcarLeitura(int planoId, int userId, AtualizarLeituraDto dto);
        Task<ProgressoGrupoResponseDto> ObterProgressoGrupo(int idPlano, int idUser);
        Task<LeituraResponseDto> ObterProgressoMe(int idPlano, int idUser);
    }
}
