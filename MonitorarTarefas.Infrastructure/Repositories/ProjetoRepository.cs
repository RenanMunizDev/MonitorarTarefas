using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.Domain.Enums;
using MonitorarTarefas.Infrastructure.Data;

public class ProjetoRepository
{
    private readonly ApplicationDbContext _context;
    public ProjetoRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public bool TemTarefasPendentes(int projetoId)
    {
        return _context.Tarefas.Any(t => t.ProjetoId == projetoId && t.Status == Status.Pendente);
    }
    public async Task<bool> RemoverProjetoAsync(int projetoId)
    {
        var projeto = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projetoId);

        if (projeto == null)
        {
            return false; 
        }
        if (TemTarefasPendentes(projetoId))
        {
            throw new InvalidOperationException("Não é possível remover o projeto, pois ele tem tarefas pendentes.");
        }
        _context.Projetos.Remove(projeto);
        await _context.SaveChangesAsync();
        return true;
    }
}
