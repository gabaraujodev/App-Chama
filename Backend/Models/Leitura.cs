namespace Backend.Models
{
    public class Leitura
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;// propiedade de navegação

        public int PlanoLeituraId { get; set; }
        public PlanoLeitura PlanoLeitura { get; set; } = null!;// propiedade de navegação

        public int UltimoCapituloLido { get; set; }

        public DateTime DataUltimaLeitura { get; set; }
    }
}
