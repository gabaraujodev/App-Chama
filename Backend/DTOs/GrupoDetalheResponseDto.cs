using Backend.Models;
using System.Collections.ObjectModel;

namespace Backend.DTOs
{
    public class GrupoDetalheResponseDto
    {

       // public int Id { get; set; } não precisa saber o id do grupo
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Lider { get; set; } = string.Empty;// Lider

        public DateTime CriadoEm { get; set; } 

        public int QuantidadeParticipantes { get; set; } = 0;

       // public List<string> ListaParticipantes { get; set; } = new List<string>();





    }
}
