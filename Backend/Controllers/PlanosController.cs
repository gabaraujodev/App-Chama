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
                var plano = await _planoLeituraService.BuscarPlanoId(planoId);
                return Ok(plano);      
        }


        [Authorize]
        [HttpPut("{planoId}")]
        public async Task<ActionResult<AtualizarPlanoLeituraDto>> EditarPlano(int planoId, AtualizarPlanoLeituraDto dto)
        {
                var planoAtualizado = await _planoLeituraService.EditarPlano(planoId, User.ObterId(), dto);
                return Ok(planoAtualizado);
        }


        [Authorize]
        [HttpDelete("{planoId}")]
        public async Task<ActionResult> DeletarPlano(int planoId)
        {
                await _planoLeituraService.DeletarPlano(planoId, User.ObterId());
                return NoContent();// Porque Deletou 204
        }
        

        [Authorize]
        [HttpPut("{planoId}/leitura")]
        public async Task<ActionResult> MarcarLeitura(int planoId,AtualizarLeituraDto dto )
        {
                await _leituraService.MarcarLeitura(planoId, User.ObterId(), dto);
                return NoContent(); 
        }


        [Authorize]
        [HttpGet("{planoId}/leitura")]
        public async Task<ActionResult> ObterProgressoMe(int planoId)
        {
                var progresso = await _leituraService.ObterProgressoMe(planoId, User.ObterId());
                return Ok(progresso);
        }


        [Authorize]
        [HttpGet("{planoId}/progresso")]
        public async Task<ActionResult> ObterProgressoGrupo(int planoId)
        {
               var progresso =  await _leituraService.ObterProgressoGrupo(planoId, User.ObterId());
               return Ok(progresso);
        }


        [Authorize]
        [HttpGet("{planoId}/participantes/progresso")]
        public async Task<ActionResult> ObterProgressoParticipantes (int planoId)
        {
                var progresso = await _planoLeituraService.ObterProgressoParticipantes(planoId, User.ObterId());
                return Ok(progresso);
        }


    }
}
