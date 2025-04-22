namespace MonitorarTarefas.API.DTOs
{
    public class HistoricoTarefaDTO
    {
        public string ?CampoModificado { get; set; } = string.Empty;
        public string ?ValorAntigo { get; set; } = string.Empty;
        public string ?ValorNovo { get; set; } = string.Empty;
        public DateTime DataAlteracao { get; set; }
        public string UsuarioResponsavel { get; set; } = string.Empty;
    }
}
