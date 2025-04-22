using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorarTarefas.Domain.Entities
{
    public class HistoricoTarefa
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public string ?CampoModificado { get; set; }
        public string ?ValorAntigo { get; set; }
        public string ?ValorNovo { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string UsuarioResponsavel { get; set; } = null!;
        public int? ComentarioId { get; set; } 
        public Comentario? Comentario { get; set; }
        public Tarefa Tarefa { get; set; } = null!;
    }
}
