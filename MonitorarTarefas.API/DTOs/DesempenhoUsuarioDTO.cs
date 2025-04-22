namespace MonitorarTarefas.DTOs
{
    public class DesempenhoUsuarioDTO
    {
        public string ?UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double MediaTarefasConcluidas { get; set; }
    }
}
