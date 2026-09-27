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
        var usuarioCriado = await _usuarioService.CriarUsuario(dto);
        return Ok(usuarioCriado);
    }


    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UsuarioResponseDto>> UsuarioAtual()
    {
        return Ok(await _usuarioService.BuscarPorId(User.ObterId()));
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletarPorId(int id)
    {
        await _usuarioService.DeletarPorId(id, User.ObterId());
        return NoContent();// Porque Deletou 204
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioResponseDto>> UpdatePorId(int id, AtualizarUsuarioDto dto)
    {
        var usuarioAtualizado = await _usuarioService.UpdatePorId(id, User.ObterId(), dto);
        return Ok(usuarioAtualizado);
    }


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginUsuarioDto dto)
    {
        var usuario = await _usuarioService.Login(dto);
        var token = _tokenService.Generate(usuario);// eu ja tratei no service se for null

        return Ok(new LoginResponseDto
        {
            Token = token,
            Usuario = usuario // eu ja tratei no service se for null
        });
    }



}
