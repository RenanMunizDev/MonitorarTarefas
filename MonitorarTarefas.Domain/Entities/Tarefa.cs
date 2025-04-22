using Microsoft.AspNetCore.Identity;
using MonitorarTarefas.Domain.Entities;
using MonitorarTarefas.Domain.Enums;

public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string ?Responsavel { get; set; }
    public bool Concluida { get; set; }
    public Status Status { get; set; }
    public Prioridade Prioridade { get; set; }
    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int UsuarioId { get; set; }
    public Usuario ?Usuario { get; set; }
    public ICollection<Comentario> ?Comentarios { get; set; }
    public ICollection<HistoricoTarefa> Historico { get; set; } = new List<HistoricoTarefa>();
}
