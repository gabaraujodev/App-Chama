using Backend.Models;

namespace Backend.DTOs
{
    public class ProgressoGrupoResponseDto
    {
        
        public int Participantes { get; set; }

        public int Concluiram {  get; set; }
        public int PercentualGrupo { get; set; }

        public List<string> ListaParticipantes { get; set; } = new List<string>();
        
    }
}
