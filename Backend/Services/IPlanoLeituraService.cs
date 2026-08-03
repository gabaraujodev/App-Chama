using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPlanoLeituraService
    {
        Task<PlanoLeituraResponseDto> CriarPlano(int idGrupo, int idUser, CriarPlanoLeituraDto dto);
        Task<List<PlanoLeituraResponseDto>> ListarPlanos(int idGrupo);
        Task<PlanoLeituraResponseDto> BuscarPlanoId(int idGrupo, int idPlano);
        Task<AtualizarPlanoLeituraDto> EditarPlano(int idGrupo, int idPlano, int idUser, AtualizarPlanoLeituraDto dto);
        Task DeletarPlano(int idGrupo, int idPlano, int idUser);
    }
}
