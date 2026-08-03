using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class GrupoService : IGrupoService
    {
        private readonly AppDbContext _context;

        public GrupoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GrupoResponseDto> CriarGrupo(CriarGrupoDto dto, int idClaim)
        {
            // Como ela pode ter o mesmo nome de outros grupos e a mesma descrição eu não preciso verificar se ja tem outro no banco
            // 1. Cria entidade
            var grupo = new Grupo
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                CriadoEm = DateTime.UtcNow,
                CriadorId = idClaim
            };

          
        
            var participante = new Participante
            {
                UsuarioId = idClaim,
                Grupo = grupo// EF coloca o id do grupo pra mim com as propiedades de navegação
            };

            _context.Grupos.Add(grupo);
            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            return new GrupoResponseDto
            {
                Id = grupo.Id,
                Nome = grupo.Nome,
                Descricao = grupo.Descricao
            };


        }
       

        public async Task EntrarNoGrupo(int idGrupo, int idUsuario)
        {
            bool grupoExiste = await _context.Grupos.AnyAsync(g => g.Id == idGrupo);
            if (!grupoExiste)
            {
                throw new NotFoundException("Grupo não encontrado.");
            }
            bool jaExiste = await _context.Participantes
                                  .AnyAsync(p => p.UsuarioId == idUsuario && p.GrupoId == idGrupo);
            if (jaExiste)
            {
                throw new ConflictException("Usuário já participa do grupo.");// tem que dar 409(conflito), ja esta no grupo
            }

            var participante = new Participante
            {
                UsuarioId = idUsuario,
                GrupoId = idGrupo
            };


            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();


        }
        public async Task SairDoGrupo(int idGrupo, int idUsuario)
        {
            if (!(await _context.Grupos.AnyAsync(p => p.Id == idGrupo)))
            {
                throw new NotFoundException("Grupo Não existe");
            }

            if (!(await _context.Participantes.AnyAsync(p => p.UsuarioId == idUsuario && p.GrupoId == idGrupo)))
            {
                throw new ConflictException("Não esta no grupo!");// retorna false porque tem que dar 409(conflito), não esta no grupo
            }


            _context.Participantes.Remove((await _context.Participantes
                                     .FirstOrDefaultAsync(p => p.GrupoId == idGrupo && p.UsuarioId == idUsuario))!); // não pode voltar nulo pq ja tratei isso antes
            await _context.SaveChangesAsync();


        }
        public async Task<List<GrupoResponseDto>> ListarGrupos()
        {
            var grupos = await _context.Grupos
                .Select
                (c => new GrupoResponseDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao
                }).ToListAsync();


            return grupos;
        }
        public async Task<List<GrupoResponseDto>> ListarGruposDoUsuario(int idUsuario)
        {
            var grupos = await _context.Grupos
                .Where(c => c.Participantes.Any(h => h.UsuarioId == idUsuario))
                .Select
                (d => new GrupoResponseDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Descricao = d.Descricao
                }).ToListAsync();


            return grupos;
        }
        public async Task<GrupoDetalheResponseDto> BuscarGrupoPorId(int idGrupo)
        {
            var grupo = await _context.Grupos
                .Where(p => p.Id == idGrupo)
                .Select
                (c => new GrupoDetalheResponseDto
                {
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    Lider = c.Criador.Nome,
                    CriadoEm = c.CriadoEm,
                    QuantidadeParticipantes = c.Participantes.Count(),
                    ListaParticipantes = c.Participantes
                   .Select(d => d.Usuario.Nome).ToList()// Ele entra em participante e seleciona somente o nome dos usuarios

                }).FirstOrDefaultAsync();
             if (grupo == null)
            {
                throw new NotFoundException("Não foram encontrados grupos!");
            }

            return grupo;
        }



    }
}
