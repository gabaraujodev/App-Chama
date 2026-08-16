namespace Backend.DTOs
{
    public class AtualizarUsuarioDto
    {
        //public int Id { get; set; } a pessoa vai passar o id na rota e nao no corpo
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
