namespace Backend.Models;

public class Usuario
{   
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Grupo> GruposCriados { get; set; }= new List<Grupo>();
    public ICollection<Participante> Participacoes { get; set; } = new List<Participante>();
}