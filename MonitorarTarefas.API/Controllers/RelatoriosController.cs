using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.API.DTOs;
using MonitorarTarefas.Infrastructure.Data;

namespace MonitorarTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("media-tarefas-concluidas")]
        public async Task<IActionResult> GetMediaTarefasConcluidas(
            [FromQuery] string funcao,
            [FromQuery] string? dataInicio = null,
            [FromQuery] string? dataFim = null)
        {
            // Verifica permissão
            if (funcao != "gerente")
            {
                return StatusCode(403, new { erro = "Acesso negado. Apenas usuários com função 'gerente' podem acessar este relatório." });
            }

            // Define período padrão (últimos 30 dias)
            var formato = "dd/MM/yyyy";
            var cultura = CultureInfo.InvariantCulture;

            DateTime dataFimParsed = string.IsNullOrEmpty(dataFim)
                ? DateTime.UtcNow
                : DateTime.ParseExact(dataFim, formato, cultura);

            DateTime dataInicioParsed = string.IsNullOrEmpty(dataInicio)
                ? dataFimParsed.AddDays(-30)
                : DateTime.ParseExact(dataInicio, formato, cultura);

            if (dataInicioParsed > dataFimParsed)
            {
                return BadRequest(new { erro = "A data de início deve ser anterior à data de fim." });
            }

            try
            {
                // Consulta para contar tarefas concluídas e usuários
                var query = await _context.Tarefas
                    .Where(t => t.DataConclusao >= dataInicioParsed && t.DataConclusao <= dataFimParsed)
                    .GroupBy(t => t.UsuarioId)
                    .Select(g => new { UsuarioId = g.Key, ContagemTarefas = g.Count() })
                    .ToListAsync();

                int totalTarefas = query.Sum(x => x.ContagemTarefas);
                int totalUsuarios = query.Count;
                double mediaTarefas = totalUsuarios > 0 ? (double)totalTarefas / totalUsuarios : 0;

                // Monta resposta
                var relatorio = new RelatorioTarefas
                {
                    Periodo = new PeriodoRelatorioDTO
                    {
                        DataInicio = dataInicioParsed.ToString("yyyy-MM-dd"),
                        DataFim = dataFimParsed.ToString("yyyy-MM-dd")
                    },
                    MediaTarefasPorUsuario = Math.Round(mediaTarefas, 2),
                    TotalUsuarios = totalUsuarios,
                    TotalTarefasConcluidas = totalTarefas
                };

                return Ok(new { relatorio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor: " + ex.Message });
            }
        }
    }
}
