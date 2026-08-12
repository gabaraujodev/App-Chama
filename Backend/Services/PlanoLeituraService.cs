using Backend.Data;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Services
{
    public class PlanoLeituraService : IPlanoLeituraService
    {
        private readonly AppDbContext _context;
        public PlanoLeituraService(AppDbContext context)
        {
            _context = context; // variavel de acesso ao DB pelo entity
        }


        public async Task<PlanoLeituraResponseDto> CriarPlano(int idGrupo, int idUser, CriarPlanoLeituraDto dto)
        {
            // Fazer todas verificaçoes, criar o plano e retornar true caso for criado, false se o grupo não exitir

            if (!(await _context.Grupos.AnyAsync(p => p.Id == idGrupo)))
            {
                throw new NotFoundException("Grupo Inexistente!");// Grupo não existe           
            }

            if (!(await _context.Grupos.AnyAsync(p => p.CriadorId == idUser && p.Id == idGrupo)))
            {
                throw new ForbiddenException("Sem Autorização!");// Quem esta criando o plano não é o Lider,(Não tem autorização)            
            }
            ValidarPlano(
            dto.Livro,
            dto.CapituloInicial,
            dto.CapituloFinal,
            dto.DataInicio,
            dto.DataFim);
            var ultimoPlano = await _context.PlanoLeitura// ordena em ordem decrescente(com base na data em que foi criado) e pega o primeiro
                             .Where(p => p.GrupoId == idGrupo)
                             .OrderByDescending(p => p.CriadoEm)
                             .FirstOrDefaultAsync();


            if (ultimoPlano != null && dto.DataInicio <= ultimoPlano.DataFim)
            {
                throw new ConflictException("O novo plano não pode começar antes que o último plano termine!");
            }

            var planoLeitura = new PlanoLeitura
            {
                GrupoId = idGrupo,
                Livro = dto.Livro,
                CapituloInicial = dto.CapituloInicial,
                CapituloFinal = dto.CapituloFinal,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                CriadoEm = DateTime.UtcNow
            };
            _context.PlanoLeitura.Add(planoLeitura);
            await _context.SaveChangesAsync();
            var planoLeituraResponse = new PlanoLeituraResponseDto
            {
                Livro = planoLeitura.Livro,
                CapituloInicial = planoLeitura.CapituloInicial,
                CapituloFinal = planoLeitura.CapituloFinal,
                DataFim = planoLeitura.DataFim,
                DataInicio = planoLeitura.DataInicio
            };
            return planoLeituraResponse;
        }

        public async Task<List<PlanoLeituraResponseDto>> ListarPlanos(int idGrupo)
        {
            var grupos = await _context.PlanoLeitura
                .Where(p => p.GrupoId == idGrupo)
                .Select
                (c => new PlanoLeituraResponseDto
                {
                    Livro = c.Livro,
                    CapituloInicial = c.CapituloInicial,
                    CapituloFinal = c.CapituloFinal,
                    DataFim = c.DataFim,
                    DataInicio = c.DataInicio
                }).ToListAsync();

            if (!grupos.Any())
            {
                throw new NotFoundException("Nenhum plano encontrado!");
            }
            return grupos;
        }
        public async Task<PlanoLeituraResponseDto> BuscarPlanoId(int idPlano)
        {
            var plano = await _context.PlanoLeitura
                .Where(p => p.Id == idPlano)
                .Select
                (c => new PlanoLeituraResponseDto
                {
                    Livro = c.Livro,
                    CapituloInicial = c.CapituloInicial,
                    CapituloFinal = c.CapituloFinal,
                    DataFim = c.DataFim,
                    DataInicio = c.DataInicio
                }).FirstOrDefaultAsync();
            if (plano == null)
            {
                throw new NotFoundException("Plano Não Encontrado");
            }

            return plano;

        }
        public async Task<AtualizarPlanoLeituraDto> EditarPlano(int idPlano, int idUser, AtualizarPlanoLeituraDto dto)
        {
            var plano = await _context.PlanoLeitura.Where(p => p.Id == idPlano)
                                                    .Include(p => p.Grupo)
                                                    .FirstOrDefaultAsync();
            if (plano == null)
            {
                throw new NotFoundException("Plano ou Grupo Não encontrado!");
            }
            if (!(plano.Grupo.CriadorId == idUser))// somente o lider do grupo pode editar planos
            {
                throw new ForbiddenException("Sem Autorização!");
            }

            ValidarPlano(
            dto.Livro,
            dto.CapituloInicial,
            dto.CapituloFinal,
            dto.DataInicio,
            dto.DataFim);
            var ultimoPlano = await _context.PlanoLeitura// ordena em ordem decrescente(com base na data em que foi criado) e pega o primeiro
                            .Where(p => p.Id != idPlano && p.GrupoId == plano.GrupoId)// ignora o plano atual e busca so no grupo selecionado
                            .OrderByDescending(p => p.CriadoEm)
                            .FirstOrDefaultAsync();


            if (ultimoPlano != null && dto.DataInicio <= ultimoPlano.DataFim)
            {
                throw new ConflictException("O novo plano não pode começar antes que o último plano termine!");
            }
            plano.CapituloInicial = dto.CapituloInicial;
            plano.CapituloFinal = dto.CapituloFinal;
            plano.Livro = dto.Livro;
            plano.DataFim = dto.DataFim;
            plano.DataInicio = dto.DataInicio;

            await _context.SaveChangesAsync();
            return new AtualizarPlanoLeituraDto
            {
                Livro = plano.Livro,
                CapituloInicial = plano.CapituloInicial,
                CapituloFinal = plano.CapituloFinal,
                DataFim = plano.DataFim,
                DataInicio = plano.DataInicio
            };
        }
        public async Task DeletarPlano(int idPlano, int idUser)
        {
            // 1. Busca o plano incluindo o grupo para validar as regras
            var plano = await _context.PlanoLeitura
                .Include(p => p.Grupo)
                .FirstOrDefaultAsync(p => p.Id == idPlano);

            // 2. Se não achou nem o plano nem o grupo
            if (plano == null)
            {
                throw new NotFoundException("Plano ou grupo não encontrado.");
            }

            // 3. Valida se quem está tentando excluir é o líder do grupo
            if (plano.Grupo.CriadorId != idUser)
            {
                throw new ForbiddenException("Somente o líder do grupo pode excluir este plano.");
            }

            // 4. Remove e salva
            _context.PlanoLeitura.Remove(plano);
            await _context.SaveChangesAsync();

        }
        public async Task<List<ParticipanteProgressoResponseDto>> ObterProgressoParticipantes(int idPlano, int idUser)
        {
            // 1. Verifica se o plano existe
            var plano = await _context.PlanoLeitura
                .Include(p => p.Grupo)
                .FirstOrDefaultAsync(p => p.Id == idPlano);

            if (plano == null)
            {
                throw new NotFoundException("Plano não existe.");
            }

            // 2. Verifica se o usuário participa do grupo desse plano
            var participa = await _context.Participantes
                .AnyAsync(p =>
                    p.GrupoId == plano.GrupoId &&
                    p.UsuarioId == idUser);

            if (!participa)
            {
                throw new ForbiddenException("Você não participa desse grupo.");
            }

            // 3. Busca todos os participantes do grupo
            var progresso = await _context.Participantes
                .Where(p => p.GrupoId == plano.GrupoId)
                .Select(p => new ParticipanteProgressoResponseDto
                {
                    UsuarioId = p.UsuarioId,
                    Nome = p.Usuario.Nome,

                    UltimoCapituloLido = _context.Leitura
                        .Where(l =>
                            l.UsuarioId == p.UsuarioId &&
                            l.PlanoLeituraId == idPlano)
                        .Select(l => (int?)l.UltimoCapituloLido)
                        .FirstOrDefault() ?? 0,

                    Percentual = _context.Leitura
                        .Where(l =>
                            l.UsuarioId == p.UsuarioId &&
                            l.PlanoLeituraId == idPlano)
                        .Select(l =>
                            l.UltimoCapituloLido > 0
                                ? (int)(((double)(l.UltimoCapituloLido - plano.CapituloInicial + 1) /
                                (double)(plano.CapituloFinal - plano.CapituloInicial + 1 )) * 100): 0)

                        .FirstOrDefault(),

                    Concluiu = _context.Leitura
                        .Where(l =>
                            l.UsuarioId == p.UsuarioId &&
                            l.PlanoLeituraId == idPlano)
                        .Select(l =>
                            l.UltimoCapituloLido >= plano.CapituloFinal)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return progresso;
        }
        private static void ValidarPlano(
       string livro,
       int capInicial,
       int capFinal,
       DateTime dataInicio,
       DateTime dataFim)
        {
            if (dataFim < dataInicio)
                throw new BadRequestException("A data final não pode ser anterior à data inicial.");

            if (capInicial > capFinal)
                throw new BadRequestException("O capítulo inicial não pode ser maior que o capítulo final.");

            if (string.IsNullOrWhiteSpace(livro))
                throw new BadRequestException("Livro Invalido!");
        }

    }

}
