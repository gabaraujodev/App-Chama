using Backend.Data;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoService _grupoService;
        public GruposController(IGrupoService grupoService)
        {
            _grupoService = grupoService;// Injeta a dependencia
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GrupoResponseDto>> CriarGrupo(CriarGrupoDto dto)
        {

            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue))
            {
                return Unauthorized("Usuário não identificado.");
            }


            var id = int.Parse(claimValue);// aqui eu tenho o ID que veio do JWT TOKEN



            try
            {
                var grupoCriado = await _grupoService.CriarGrupo(dto, id);

                return Ok(grupoCriado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }

        }

        [Authorize]
        [HttpPost("{id}/entrar")]
        public async Task<ActionResult> EntrarNoGrupo(int id)
        {

            if (!(await _grupoService.GrupoExiste(id)))
            {
                return NotFound(); // 404
            }

            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue))
            {
                return Unauthorized("Usuário não identificado.");
            }


            var idUser = int.Parse(claimValue);// aqui eu tenho o ID que veio do JWT TOKEN(usuario logado)
            bool entrou = await _grupoService.EntrarNoGrupo(id, idUser);
            if (!entrou)
            {
                return Conflict(new { messagem = "Usuário já participa do grupo." });// 
            }
            return NoContent();// cricou com sucesso


        }
        [Authorize]
        [HttpPost("{id}/sair")]
        public async Task<ActionResult> SairDoGrupo(int id)
        {
            if (!(await _grupoService.GrupoExiste(id)))
            {
                return NotFound(); // 404
            }

            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue))
            {
                return Unauthorized("Usuário não identificado.");
            }


            var idUser = int.Parse(claimValue);// aqui eu tenho o ID que veio do JWT TOKEN(usuario logado)
            bool saiu = await _grupoService.SairDoGrupo(id, idUser);
            if (!saiu)
            {
                return Conflict(new { messagem = "Usuário Não esta no grupo." });// 
            }
            return NoContent();// executou com sucesso


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
            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue))
            {
                return Unauthorized("Usuário não identificado.");
            }


            var id = int.Parse(claimValue);// aqui eu tenho o ID que veio do JWT TOKEN

            var grupos = await _grupoService.ListarGruposDoUsuario(id);

            return Ok(grupos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoDetalheResponseDto>> BuscarGrupoPorId(int id)
        {
            if (!(await _grupoService.GrupoExiste(id)))
            {
                return NotFound(); // 404
            }

            var grupoDetalhado = await _grupoService.BuscarGrupoPorId(id);
            return Ok(grupoDetalhado);

        }
    }
}

