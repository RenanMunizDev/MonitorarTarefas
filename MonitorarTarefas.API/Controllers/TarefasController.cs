using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.API.DTOs;
using MonitorarTarefas.Domain.Entities;
using MonitorarTarefas.Domain.Enums;
using MonitorarTarefas.Infrastructure.Data;

namespace MonitorarTarefas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public TarefasController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterTarefa(int id)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tarefa == null)
        {
            return NotFound("Tarefa não encontrada.");
        }
        var tarefaDTO = _mapper.Map<TarefaDTO>(tarefa);
        return Ok(tarefaDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarTarefa(TarefaDTO tarefaDTO)
        {
            var projetoExistente = await _context.Projetos
                .Include(p => p.Tarefas)
                .FirstOrDefaultAsync(p => p.Id == tarefaDTO.ProjetoId);

            if (projetoExistente == null)
            {
                return NotFound("Projeto não encontrado.");
            }

            if (projetoExistente.Tarefas.Count >= 20)
            {
                return BadRequest("O projeto atingiu o limite máximo de 20 tarefas.");
            }

        var novaTarefa = _mapper.Map<Tarefa>(tarefaDTO);
        _context.Tarefas.Add(novaTarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterTarefa), new { id = novaTarefa.Id }, novaTarefa);
        }


    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarTarefa(int id, TarefaDTO tarefaDTO)
    {
        var tarefaExistente = await _context.Tarefas.FindAsync(id);
        if (tarefaExistente == null)
        {
            return NotFound();
        }

        if (tarefaExistente.Prioridade != (Prioridade)tarefaDTO.Prioridade)
        {
            return BadRequest("A prioridade da tarefa não pode ser alterada após a criação.");
        }

        var historicos = new List<HistoricoTarefa>();
        var usuario = User.Identity?.Name ?? "Desconhecido";

        if (tarefaExistente.Titulo != tarefaDTO.Titulo)
        {
            historicos.Add(new HistoricoTarefa
            {
                TarefaId = tarefaExistente.Id,
                CampoModificado = "Titulo",
                ValorAntigo = tarefaExistente.Titulo,
                ValorNovo = tarefaDTO.Titulo,
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario
            });
        }

        if (tarefaExistente.Descricao != tarefaDTO.Descricao)
        {
            historicos.Add(new HistoricoTarefa
            {
                TarefaId = tarefaExistente.Id,
                CampoModificado = "Descricao",
                ValorAntigo = tarefaExistente.Descricao,
                ValorNovo = tarefaDTO.Descricao,
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario
            });
        }

        if (tarefaExistente.Responsavel != tarefaDTO.Responsavel)
        {
            historicos.Add(new HistoricoTarefa
            {
                TarefaId = tarefaExistente.Id,
                CampoModificado = "Responsavel",
                ValorAntigo = tarefaExistente.Responsavel,
                ValorNovo = tarefaDTO.Responsavel,
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario
            });
        }

        if (tarefaExistente.Concluida != tarefaDTO.Concluida)
        {
            historicos.Add(new HistoricoTarefa
            {
                TarefaId = tarefaExistente.Id,
                CampoModificado = "Concluida",
                ValorAntigo = tarefaExistente.Concluida.ToString(),
                ValorNovo = tarefaDTO.Concluida.ToString(),
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario
            });
        }

        if (tarefaExistente.Status != tarefaDTO.Status)
        {
            historicos.Add(new HistoricoTarefa
            {
                TarefaId = tarefaExistente.Id,
                CampoModificado = "Status",
                ValorAntigo = tarefaExistente.Status.ToString(),
                ValorNovo = tarefaDTO.Status.ToString(),
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario
            });
        }

        tarefaExistente.Titulo = tarefaDTO.Titulo;
        tarefaExistente.Descricao = tarefaDTO.Descricao;
        tarefaExistente.Responsavel = tarefaDTO.Responsavel;
        tarefaExistente.Concluida = tarefaDTO.Concluida;
        tarefaExistente.Status = tarefaDTO.Status;

        _context.HistoricoTarefas.AddRange(historicos);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpGet("{id}/historico")]
    public async Task<IActionResult> ObterHistoricoTarefa(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        var historico = await _context.HistoricoTarefas
            .Where(h => h.TarefaId == id)
            .OrderByDescending(h => h.DataAlteracao)
            .Select(h => new HistoricoTarefaDTO
            {
                CampoModificado = h.CampoModificado,
                ValorAntigo = h.ValorAntigo,
                ValorNovo = h.ValorNovo,
                DataAlteracao = h.DataAlteracao,
                UsuarioResponsavel = h.UsuarioResponsavel
            })
            .ToListAsync();

        return Ok(historico);
    }

    [HttpDelete("projetos/{projetoId}/tarefas/{tarefaId}")]
    public async Task<IActionResult> RemoverTarefaDeProjeto(int projetoId, int tarefaId)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == tarefaId && t.ProjetoId == projetoId);

        if (tarefa == null)
        {
            return NotFound("Tarefa não encontrada para este projeto.");
        }

        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{tarefaId}/comentarios")]
    //[Authorize]
    public async Task<IActionResult> AdicionarComentario(int tarefaId, [FromBody] AdicionarComentarioDTO dto)
    {
        var tarefa = await _context.Tarefas.FindAsync(tarefaId);
        if (tarefa == null)
            return NotFound("Tarefa não encontrada.");

        // Criar o comentário
        var comentario = new Comentario
        {
            TarefaId = tarefaId,
            Autor = dto.Autor,
            Texto = dto.Texto,
            DataCriacao = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        // Registrar histórico
        var historico = new HistoricoTarefa
        {
            TarefaId = tarefaId,
            DataAlteracao = DateTime.UtcNow,
            UsuarioResponsavel = dto.Autor,
            ComentarioId = comentario.Id
        };

        _context.HistoricoTarefas.Add(historico);
        await _context.SaveChangesAsync();

        return Ok("Comentário adicionado e registrado no histórico.");
    }




}
