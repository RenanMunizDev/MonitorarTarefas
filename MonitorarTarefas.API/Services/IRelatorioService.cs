using MonitorarTarefas.DTOs;

namespace MonitorarTarefas.Services.Interfaces
{
    public interface IRelatorioService
    {
        Task<List<DesempenhoUsuarioDTO>> ObterDesempenhoUsuariosAsync();
    }
}
