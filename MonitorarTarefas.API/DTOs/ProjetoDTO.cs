using MonitorarTarefas.Domain.Enums;

public class ProjetoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Status Status { get; set; }
    public List<TarefaDTO> Tarefas { get; set; } = new List<TarefaDTO>();
}
