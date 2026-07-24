namespace Backend.Models
{
    public class Participante
    {
      
        public int UsuarioId { get; set; }
        public int GrupoId { get; set; }
        public DateTime EntrouEm { get; set; } = DateTime.UtcNow;

        public Usuario Usuario { get; set; } = null!;

        public Grupo Grupo { get; set; } = null!;

       
}
}
