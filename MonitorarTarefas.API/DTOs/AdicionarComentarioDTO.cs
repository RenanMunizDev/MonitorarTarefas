namespace MonitorarTarefas.API.DTOs
{
    public class AdicionarComentarioDTO
    {
        public int UsuarioId { get; set; }

        public string ?Autor { get; set; }
        public string ?Texto { get; set; }
    }
}
