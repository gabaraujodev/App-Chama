using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Participante> Participantes { get; set; }
    public DbSet<PlanoLeitura> PlanoLeitura { get; set; }
    public DbSet<Leitura> Leitura { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participante>()
           .HasKey(e=> new {e.GrupoId, e.UsuarioId});
        modelBuilder.Entity<Participante>()
            .HasOne(s => s.Usuario)
            .WithMany(f => f.Participacoes)
            .HasForeignKey(f => f.UsuarioId);
        modelBuilder.Entity<Participante>()
            .HasOne(s => s.Grupo)
            .WithMany(f=>f.Participantes)
            .HasForeignKey(g=>g.GrupoId);

        modelBuilder.Entity<Grupo>()
            .HasOne(s => s.Criador)
            .WithMany(h => h.GruposCriados)
            .HasForeignKey(j=>j.CriadorId);

        modelBuilder.Entity<Leitura>()
        .HasKey(l => l.Id);

        modelBuilder.Entity<Leitura>()
            .HasOne(l => l.Usuario)
            .WithMany(f=> f.Leituras) // Se quiser criar uma collection na classe Usuario, pode colocar aqui
            .HasForeignKey(l => l.UsuarioId);

        modelBuilder.Entity<Leitura>()
            .HasOne(l => l.PlanoLeitura)
            .WithMany(f => f.Leituras) // Se quiser criar uma collection na classe PlanoLeitura, pode colocar aqui
            .HasForeignKey(l => l.PlanoLeituraId);
    }
}