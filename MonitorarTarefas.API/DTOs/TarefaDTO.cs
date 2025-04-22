using MonitorarTarefas.Domain.Enums;

public class TarefaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? Responsavel { get; set; }
    public bool Concluida { get; set; }
    public Status Status { get; set; }
    public int Prioridade { get; set; } 
    public int ProjetoId { get; set; }
    public int UsuarioId { get; set; }
}
