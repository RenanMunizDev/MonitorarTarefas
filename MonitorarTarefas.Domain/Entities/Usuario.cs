using System;

namespace MonitorarTarefas.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string ?Funcao { get; set; }
        public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}