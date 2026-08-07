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
        public class PlanosController : ControllerBase
        {
            private readonly ILeituraService _leituraService;
            private readonly IPlanoLeituraService _planoLeituraService;
            public PlanosController(ILeituraService leituraService, IPlanoLeituraService planoLeituraService)
            {
                _leituraService = leituraService;// Injeta a dependencia
                _planoLeituraService = planoLeituraService;
            }

        [Authorize]
        [HttpGet("{planoId}")]
        public async Task<ActionResult> BuscarPlanoId(int planoId)
        {

            try
            {
                var plano = await _planoLeituraService.BuscarPlanoId(planoId);
                return Ok(plano);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{planoId}")]
        public async Task<ActionResult<AtualizarPlanoLeituraDto>> EditarPlano(int planoId, AtualizarPlanoLeituraDto dto)
        {
            try
            {
                var planoAtualizado = await _planoLeituraService.EditarPlano(planoId, User.ObterId(), dto);
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
        [HttpDelete("{planoId}")]
        public async Task<ActionResult> DeletarPlano(int planoId)
        {
            try
            {
                await _planoLeituraService.DeletarPlano(planoId, User.ObterId());
                return NoContent();// Porque Deletou 204
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(403, new
                {
                    erro = ex.Message
                });// forbid() não retorna mensagem , entao tem que retornar assim
            }

           
        }
        /*
            POST   /api/planos/{id}/marcar-leitura

            GET    /api/planos/{id}/progresso

            GET    /api/planos/{id}/participantes

            GET    /api/planos/{id}/pendentes*/
        [Authorize]
        [HttpPut("{planoId}/leitura")]
        public async Task<ActionResult> MarcarLeitura(int planoId,AtualizarLeituraDto dto )
        {
            try
            {
                await _leituraService.MarcarLeitura(planoId, User.ObterId(), dto);
                return NoContent();
            }
            catch (NotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
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
        [HttpGet("{planoId}/leitura")]
        public async Task<ActionResult> ObterProgressoMe(int planoId)
        {
            try
            {
                var progresso = await _leituraService.ObterProgressoMe(planoId, User.ObterId());
                return Ok(progresso);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{planoId}/progresso")]
        public async Task<ActionResult> ObterProgresso(int planoId)
        {
            try
            {
               var progresso =  await _leituraService.ObterProgressoGrupo(planoId, User.ObterId());
               return Ok(progresso);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
