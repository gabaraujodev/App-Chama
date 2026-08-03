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
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<UsuarioResponseDto>>> ListarUsuarios()
    {
        var usuarios = await _usuarioService.ListarUsuarios();

        return Ok(usuarios);
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

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> BuscarPorId(int id)
    {

        try
        {
            return Ok(await _usuarioService.BuscarPorId(id));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarPorId(int id)
    {
        try
        {
            await _usuarioService.DeletarPorId(id);
            return NoContent();// Porque Deletou 204
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }


    }



    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> UpdatePorId(int id, AtualizarUsuarioDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest("ID da rota diferente do corpo");
            }

            var usuarioAtualizado = await _usuarioService.UpdatePorId(dto);
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