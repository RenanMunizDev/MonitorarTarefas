namespace MonitorarTarefas.API.DTOs
{
    public class RelatorioTarefas
    {
        public PeriodoRelatorioDTO ?Periodo { get; set; }
        public double MediaTarefasPorUsuario { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalTarefasConcluidas { get; set; }
    }
}
