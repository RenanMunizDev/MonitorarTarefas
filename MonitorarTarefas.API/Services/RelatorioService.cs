using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.Domain.Enums;
using MonitorarTarefas.DTOs;
using MonitorarTarefas.Infrastructure.Data;
using MonitorarTarefas.Services.Interfaces;

namespace MonitorarTarefas.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly ApplicationDbContext _context;

        public RelatorioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DesempenhoUsuarioDTO>> ObterDesempenhoUsuariosAsync()
        {
            var dataLimite = DateTime.UtcNow.AddDays(-30);

            var resultado = await _context.Tarefas
                .Include(t => t.Usuario)
                .Where(t => t.Status == Status.Concluida && t.DataConclusao >= dataLimite)
                .GroupBy(t => new { t.Usuario!.Id, t.UsuarioId })
                .Select(g => new DesempenhoUsuarioDTO
                {
                    //UsuarioId = g.Key.UsuarioId,
                    //Nome = g.Key.Id,
                    MediaTarefasConcluidas = g.Count() / 30.0
                })
                .ToListAsync();

            return resultado;
        }
    }
}
