using Backend.DTOs;
using Backend.Exceptions;
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
        var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(claimValue))
        {
            return Unauthorized("Usuário não identificado.");
        }

        
        var id = int.Parse(claimValue);// aqui eu tenho o ID que veio do JWT TOKEN

        var usuario = await _usuarioService.BuscarPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> BuscarPorId(int id)
    {
        var usuario = await _usuarioService.BuscarPorId(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return Ok(usuario);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarPorId(int id) {
        var resultado = await _usuarioService.DeletarPorId(id);
        if (resultado)
        {
            return NoContent();// Porque Deletou 204
        }
        else
            return NotFound();// Ñão encontrou 404
        
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

            if (usuarioAtualizado == null)
            {
                return NotFound();
            }
            return Ok(usuarioAtualizado);
        }catch(Exception ex)
{
            return Conflict(new { erro = ex.Message });
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.Login(dto);
            if (usuario == null)
            {
                return NotFound();
            }
            var token = _tokenService.Generate(usuario);
          
            return Ok(new LoginResponseDto
            {
                Token = token,
                Usuario = usuario
            });
        }

        
        catch (UnauthorizedException ex)
        {
        {
            return Unauthorized(new { erro = ex.Message });
        }
    }
    }
}