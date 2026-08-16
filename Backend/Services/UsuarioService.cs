using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Exceptions;
using Microsoft.EntityFrameworkCore;


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
            throw new ConflictException("Email já está em uso");

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
            Id = usuario.Id,// Ganhou Id depois de Ser Adicionado no Banco
            Nome = usuario.Nome,
            Email = usuario.Email,
            CriadoEm = usuario.CriadoEm
        };
    }


    public async Task<UsuarioResponseDto> BuscarPorId(int id)
    {
       
        var usuario = await _context.Usuarios
            .Where(p => p.Id == id) // Where com o mesmo ID recebido do get
            .Select(user => new UsuarioResponseDto // select o usuario do bd
            {
                Id = user.Id,   
                Nome = user.Nome,
                Email = user.Email,
                CriadoEm = user.CriadoEm

            })
            .FirstOrDefaultAsync(); // Essa função retorna o primeiro ou se não achar nada NULL
        if(usuario == null)
        {
            throw new NotFoundException("Usuario não encontrado!");
        }
        return usuario;
    }

    public async Task DeletarPorId(int id, int idUser)
    {
        if (id != idUser)
        {
            throw new ForbiddenException("Você não tem permissão para fazer isso!");
        }
        int linhasAfetadas = await _context.Usuarios
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        if (linhasAfetadas == 0)
        {
            throw new NotFoundException("Usuário não encontrado!");
        }
    }

    public async Task<UsuarioResponseDto> UpdatePorId(int id,int idUser,AtualizarUsuarioDto dto)
    {
        if (id != idUser)
        {
            throw new BadRequestException("ID da rota diferente do logado");
        }
        var usr =  await _context.Usuarios.FindAsync(id);
        
        if (usr == null)        
        {
            throw new NotFoundException("Usuario Não encontrado");
        }
        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailExiste)
        {
            throw new ConflictException("Email já está em uso");
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

    public async Task<UsuarioResponseDto> Login(LoginUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null)
        {
             throw new UnauthorizedException("Email ou senha invalidos");
        }
        if (BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                CriadoEm= usuario.CriadoEm
            };
        }
        throw new UnauthorizedException("Email ou senha inválidos");
        
    }
    
    



}