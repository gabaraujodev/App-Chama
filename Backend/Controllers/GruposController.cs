using Backend.DTOs;
using Backend.Exceptions;
using Backend.Extensions;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoService _grupoService;
        private readonly IPlanoLeituraService _planoLeituraService;
        public GruposController(IGrupoService grupoService, IPlanoLeituraService planoLeituraService)
        {
            _grupoService = grupoService;// Injeta a dependencia
            _planoLeituraService = planoLeituraService;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GrupoResponseDto>> CriarGrupo(CriarGrupoDto dto)
        {
                var grupoCriado = await _grupoService.CriarGrupo(dto, User.ObterId());
                return Ok(grupoCriado);
        }


        [Authorize]
        [HttpPost("{id}/entrar")]
        public async Task<ActionResult> EntrarNoGrupo(int id)
        {
                await _grupoService.EntrarNoGrupo(id, User.ObterId());
                return NoContent();// criou com sucesso
        }


        [Authorize]
        [HttpPost("{id}/sair")]
        public async Task<ActionResult> SairDoGrupo(int id)
        {
                await _grupoService.SairDoGrupo(id, User.ObterId());
                return NoContent();// saiu com sucesso
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GrupoResponseDto>>> ListarGrupos()
        {
            var grupos = await _grupoService.ListarGrupos();
            return Ok(grupos);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<List<GrupoResponseDto>>> ListarGruposDoUsuario()
        {
                var grupos = await _grupoService.ListarGruposDoUsuario(User.ObterId());
                return Ok(grupos);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoDetalheResponseDto>> BuscarGrupoPorId(int id)
        {
              var grupoDetalhado = await _grupoService.BuscarGrupoPorId(id);
                return Ok(grupoDetalhado);
        }


        [Authorize]
        [HttpPost("{id}/planos")]
        public async Task<ActionResult> CriarPlano(int id, CriarPlanoLeituraDto dto)
        {   
                var plano = await _planoLeituraService.CriarPlano(id, User.ObterId(), dto);
                return Ok(plano);
        }


        [Authorize]
        [HttpGet("{id}/planos")] // Listar os planos do Grupo 
        public async Task<ActionResult> ListarPlanos(int id)
        {
                var plano = await _planoLeituraService.ListarPlanos(id);
                return Ok(plano);
        }


        [Authorize]
        [HttpGet("{id}/participantes")] // Listar os planos do Grupo 
        public async Task<ActionResult> ListaParticipantes(int id)
        {
                var plano = await _grupoService.ListarParticipantes(id,User.ObterId());
                return Ok(plano);        
        }


        [Authorize]
        [HttpDelete("{idGrupo}/participantes/{idUserDelete}")] // deletar um usuario especifico
        public async Task<ActionResult> RetirarUsuarioGrupo(int idGrupo, int idUserDelete)
        {
                await _grupoService.RetirarUsuarioGrupo(idGrupo,idUserDelete, User.ObterId());
                return NoContent();
        }


    }
}

