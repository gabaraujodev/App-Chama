namespace Backend.Models
{
    public class PlanoLeitura
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string Livro { get; set; } = string.Empty;
        public int CapituloInicial { get; set; }  
        public int CapituloFinal { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public Grupo Grupo { get; set; } = null!;

    }
}
