using Backend.Data;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Extensions;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

            try
            {
                var grupoCriado = await _grupoService.CriarGrupo(dto, User.ObterId());

                return Ok(grupoCriado);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [Authorize]
        [HttpPost("{id}/entrar")]
        public async Task<ActionResult> EntrarNoGrupo(int id)
        {

            try
            {

                await _grupoService.EntrarNoGrupo(id, User.ObterId());

                return NoContent();// criou com sucesso
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // 404
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }

        }
        [Authorize]
        [HttpPost("{id}/sair")]
        public async Task<ActionResult> SairDoGrupo(int id)
        {
            try
            {

                await _grupoService.SairDoGrupo(id, User.ObterId());

                return NoContent();// saiu com sucesso
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // 404
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }



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

            try
            {
                var grupos = await _grupoService.ListarGruposDoUsuario(User.ObterId())
;
                return Ok(grupos);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoDetalheResponseDto>> BuscarGrupoPorId(int id)
        {
            try
            {
                var grupoDetalhado = await _grupoService.BuscarGrupoPorId(id);
                return Ok(grupoDetalhado);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("{id}/planos")]
        public async Task<ActionResult> CriarPlano(int id, CriarPlanoLeituraDto dto)
        {

            try
            {
                var plano = await _planoLeituraService.CriarPlano(id, User.ObterId(), dto);
                return Ok(plano);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new
                {
                    erro = ex.Message
                });// forbid() não retorna mensagem , entao tem que retornar assim
            }



        }

        [Authorize]
        [HttpGet("{id}/planos")]
        public async Task<ActionResult> ListarPlanos(int id)
        {
            try
            {
                var plano = await _planoLeituraService.ListarPlanos(id);
                return Ok(plano);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}/planos/{planoId}")]
        public async Task<ActionResult> BuscarPlanoId(int id, int planoId)
        {

            try
            {
                var plano = await _planoLeituraService.BuscarPlanoId(id, planoId);
                return Ok(plano);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}/planos/{planoId}")]
        public async Task<ActionResult<AtualizarPlanoLeituraDto>> EditarPlano(int id, int planoId, AtualizarPlanoLeituraDto dto)
        {
            try
            {
                var planoAtualizado = await _planoLeituraService.EditarPlano(id, planoId, User.ObterId(), dto);
                return Ok(planoAtualizado);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new
                {
                    erro = ex.Message
                });// forbid() não retorna mensagem , entao tem que retornar assim
            }




        }
        [Authorize]
        [HttpDelete("{id}/planos/{planoId}")]
        public async Task<ActionResult> DeletarPlano(int id, int planoId)
        {
            try
            {
                await _planoLeituraService.DeletarPlano(id,planoId,User.ObterId());
                return NoContent();// Porque Deletou 204
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new
                {
                    erro = ex.Message
                });// forbid() não retorna mensagem , entao tem que retornar assim
            }


        }
    }
}

