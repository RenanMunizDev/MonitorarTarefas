using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.Domain.Entities;
using MonitorarTarefas.Domain.Enums;
using MonitorarTarefas.Infrastructure.Data;

namespace MonitorarTarefas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjetosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProjetosController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Projeto>>> ObterProjetos()
        => await _context.Projetos.Include(p => p.Tarefas).ToListAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjeto(int id)
    {
        var projeto = await _context.Projetos
            .Include(p => p.Tarefas) 
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projeto == null)
        {
            return NotFound();
        }
        var projetoDTO = _mapper.Map<ProjetoDTO>(projeto);
        return Ok(projetoDTO);
    }

    [HttpPost]
    public async Task<IActionResult> CriarProjeto([FromBody] ProjetoDTO projetoDTO)
    {
        var projeto = new Projeto
        {
            Nome = projetoDTO.Nome,
            Descricao = projetoDTO.Descricao,
            Status = projetoDTO.Status, 
            Tarefas = projetoDTO.Tarefas.Select(t => new Tarefa
            {
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Concluida = t.Concluida,
                Prioridade = (Prioridade)t.Prioridade
            }).ToList()
        };

        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterProjetos), new { id = projeto.Id }, projeto);
    }

    [HttpGet("{projetoId}/tarefas")]
    public async Task<ActionResult<IEnumerable<Tarefa>>> ObterTarefasDoProjeto(int projetoId)
        => await _context.Tarefas.Where(t => t.ProjetoId == projetoId).ToListAsync();

    [HttpPost("{projetoId}/tarefas")]
    public async Task<ActionResult<Tarefa>> CriarTarefa(int projetoId, [FromBody] Tarefa tarefa)
    {
        if (tarefa == null)
            return BadRequest("A tarefa não pode ser nula.");

        var projetoExiste = await _context.Projetos.AnyAsync(p => p.Id == projetoId);
        if (!projetoExiste)
            return NotFound("Projeto não encontrado.");

        var totalTarefas = await _context.Tarefas
            .CountAsync(t => t.ProjetoId == projetoId);

        if (totalTarefas >= 20)
            return BadRequest("Este projeto já atingiu o limite de 20 tarefas.");

        tarefa.ProjetoId = projetoId;
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterTarefasDoProjeto), new { projetoId }, tarefa);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarProjeto(int id)
    {
        var projeto = await _context.Projetos
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projeto == null)
        {
            return NotFound("Projeto não encontrado.");
        }

        var tarefasPendentes = projeto.Tarefas.Any(t => !t.Concluida);
        if (tarefasPendentes)
        {
            return BadRequest("Não é possível remover o projeto. Existem tarefas pendentes. Conclua ou remova as tarefas antes.");
        }
        _context.Projetos.Remove(projeto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
