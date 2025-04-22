using MonitorarTarefas.Domain.Enums;

namespace MonitorarTarefas.Domain.Entities;

public class Projeto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;  
    public Status Status { get; set; }
    public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
