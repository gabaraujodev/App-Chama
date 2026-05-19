using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
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

    [HttpGet]
    public async Task<ActionResult<List<UsuarioResponseDto>>> ListarUsuarios()
    {
        var usuarios = await _usuarioService.ListarUsuarios();

        return Ok(usuarios);
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
}