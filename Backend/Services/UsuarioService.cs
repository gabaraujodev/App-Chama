using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Linq.Expressions;

namespace Backend.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context; // Variavel de Acesso ao BD pelo Entity
    }

    public async Task<UsuarioResponseDto> CriarUsuario(CriarUsuarioDto dto)
    {
        // 1. Verifica email duplicado
        var existe = await _context.Usuarios
            .AnyAsync(u => u.Email == dto.Email);

        if (existe)
            throw new Exception("Email já está em uso");

        // 2. Cria hash da senha
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        // 3. Cria entidade
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = senhaHash,
            CriadoEm = DateTime.UtcNow
        };

        // 4. Salva no Banco
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // 5. Retorna resposta (sem Senha)
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            CriadoEm = usuario.CriadoEm
        };
    }

    public async Task<List<UsuarioResponseDto>> ListarUsuarios()
    {
        return await _context.Usuarios
            .Select(usuario => new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CriadoEm = usuario.CriadoEm
            })
            .ToListAsync();
    }
    public async Task<UsuarioResponseDto?> BuscarPorId(int id)
    {
       
        return await _context.Usuarios
            .Where(p => p.Id == id) // Where com o mesmo ID recebido do get
            .Select(user => new UsuarioResponseDto // select o usuario do bd
            {
                Id = user.Id,   
                Nome = user.Nome,
                Email = user.Email,
                CriadoEm = user.CriadoEm

            })
            .FirstOrDefaultAsync(); // Essa função retorna o primeiro ou se não achar nada NULL
    }

    public async Task<bool> DeletarPorId(int id)
    {
        int linhasAfetadas = 0;
            linhasAfetadas = await _context.Usuarios.Where(p => p.Id == id)
                                    .ExecuteDeleteAsync();

        return linhasAfetadas > 0;

    }

    public async Task<UsuarioResponseDto?> UpdatePorId(AtualizarUsuarioDto dto)
    {
        
        var usr =  await _context.Usuarios.FindAsync(dto.Id);
        
        if (usr == null)        
        {
            return null;
        }
        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != dto.Id);
        if (emailExiste)
        {
            throw new Exception("Email já está em uso");
        }
        usr.Nome = dto.Nome;
        usr.Email = dto.Email;
        await _context.SaveChangesAsync();
        return new UsuarioResponseDto
        {
            Id = usr.Id,
            Nome = usr.Nome,
            Email = usr.Email,
            CriadoEm = usr.CriadoEm
        };

    }




}