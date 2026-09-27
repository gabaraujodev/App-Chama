using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Exceptions;

namespace Backend.Services
{
    public class LeituraService : ILeituraService
    {
        private readonly AppDbContext _context;
        public LeituraService(AppDbContext context)
        {
            _context = context;
        }
        private async Task<PlanoLeitura> ObterPlanoSeParticipante(int planoId,int userId)
        {
            var plano = await _context.PlanoLeitura
                               .Include(p => p.Grupo)
                               .ThenInclude(g => g.Participantes)
                               .FirstOrDefaultAsync(p => p.Id == planoId);

            if (plano == null)
                throw new NotFoundException("Plano não encontrado.");

            if (!plano.Grupo.Participantes.Any(p => p.UsuarioId == userId))
                throw new ForbiddenException("Você não participa desse grupo.");

            return plano;
        }

        public async Task MarcarLeitura(int planoId, int userId, AtualizarLeituraDto dto)
        {
            var planoLeitura = await ObterPlanoSeParticipante (planoId, userId);
            
            var leitura = await _context.Leitura
                .Where(p => p.PlanoLeituraId == planoId && p.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (dto.UltimoCapituloLido > planoLeitura.CapituloFinal || dto.UltimoCapituloLido < planoLeitura.CapituloInicial)
            {
                throw new BadRequestException("informação invalida!");// verifica, capitulo maior que o final,meno que o inicial e 0/negativo
            }
            if (leitura == null)
            {// caso nunca tiver criado a leitura
                var leituraNovo = new Leitura
                {
                    UsuarioId = userId,
                    UltimoCapituloLido = dto.UltimoCapituloLido,
                    PlanoLeituraId = planoId,
                    DataUltimaLeitura = DateTime.UtcNow

                };
                _context.Leitura.Add(leituraNovo);
                leitura = leituraNovo;
            }
           
            leitura.UltimoCapituloLido = dto.UltimoCapituloLido;
            leitura.DataUltimaLeitura = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task<ProgressoGrupoResponseDto> ObterProgressoGrupo(int idPlano, int idUser)
        {
            await ObterPlanoSeParticipante(idPlano, idUser);
            var progresso = await _context.PlanoLeitura
        .Where(p => p.Id == idPlano)
        .Include(p => p.Grupo)
            .ThenInclude(g => g.Participantes)
        .Select(c => new ProgressoGrupoResponseDto
        {
            Participantes = c.Grupo.Participantes.Count,

            // Conta quantos participantes do grupo atingiram o capítulo final deste plano
            Concluiram = c.Grupo.Participantes
            .Count(participante => _context.Leitura
                .Any(leitura => leitura.PlanoLeituraId == c.Id
                             && leitura.UsuarioId == participante.UsuarioId
                             && leitura.UltimoCapituloLido >= c.CapituloFinal)
            ),
            ListaParticipantes = c.Grupo.Participantes.Select(d => d.Usuario.Nome).ToList()

        })
        .FirstOrDefaultAsync();
            if (progresso == null)
            {
                throw new NotFoundException("Plano sem progresso");// acho q ta errado!
            }
            progresso.PercentualGrupo = 0;
            if (progresso.Participantes > 0 )
            {
                progresso.PercentualGrupo = (int)(((double)progresso.Concluiram / progresso.Participantes) * 100);
            }



            return progresso;

        }

        public async Task<LeituraResponseDto> ObterProgressoMe(int idPlano, int idUser)
        {

            await ObterPlanoSeParticipante(idPlano, idUser);

            var progresso = await _context.PlanoLeitura
                                    .Include(p => p.Leituras)
                                    .Where(p => p.Id == idPlano)
                                    .Select(c => new LeituraResponseDto
        {
            Livro = c.Livro,
            CapituloFinal = c.CapituloFinal,
            CapituloInicial = c.CapituloInicial,
            UltimoCapituloLido = c.Leituras
                .Where(l => l.UsuarioId == idUser)
                .Select(l => (int?)l.UltimoCapituloLido)
                .FirstOrDefault() ?? 0

        }).FirstOrDefaultAsync();


            if (progresso == null)
            {
                throw new NotFoundException("Plano não encontrado.");
            }
            if (progresso.UltimoCapituloLido > 0)
            {
                // 1. Calcula a quantidade de capítulos lidos e o total de capítulos
                double capitulosLidos = (progresso.UltimoCapituloLido - progresso.CapituloInicial) + 1;
                double totalCapitulos = (progresso.CapituloFinal - progresso.CapituloInicial) + 1;

                // 2. Faz a divisão usando double e multiplica por 100
                progresso.Percentual = (int)((capitulosLidos / totalCapitulos) * 100);
            }



            return progresso;

        }
    }
}
