namespace Backend.Models;

public class Grupo
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public int CriadorId { get; set; } 

    public Usuario Criador { get; set; } = null!;

    public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}


    