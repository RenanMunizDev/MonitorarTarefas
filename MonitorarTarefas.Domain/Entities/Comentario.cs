using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorarTarefas.Domain.Entities
{
    public class Comentario
    {
        public int Id { get; set; }
        public string ?Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        public string ?Autor { get; set; } 
        public int TarefaId { get; set; }
    }


}
