using Backend.DTOs;
using Backend.Exceptions;
using Backend.Extensions;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public UsuariosController(IUsuarioService usuarioService, ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;

    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> CriarUsuario(CriarUsuarioDto dto)
    {
        try
        {
            var usuarioCriado = await _usuarioService.CriarUsuario(dto);

            return Ok(usuarioCriado);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UsuarioResponseDto>> UsuarioAtual()
    {
        try
        {
            return Ok(await _usuarioService.BuscarPorId(User.ObterId()));
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarPorId(int id)
    {
        try
        {
            await _usuarioService.DeletarPorId(id, User.ObterId());
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


    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> UpdatePorId(int id, AtualizarUsuarioDto dto)
    {
        try
        {
            

            var usuarioAtualizado = await _usuarioService.UpdatePorId(id,User.ObterId(), dto);
            return Ok(usuarioAtualizado);
        }
        catch (ConflictException ex)
        {
            return Conflict(new { erro = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.Login(dto);
            var token = _tokenService.Generate(usuario);// eu ja tratei no service se for null

            return Ok(new LoginResponseDto
            {
                Token = token,
                Usuario = usuario // eu ja tratei no service se for null
            });
        }


        catch (UnauthorizedException ex)
        {
            {
                return Unauthorized(ex.Message );
            }
        }
    }
}