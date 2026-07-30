using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class SolicitacaoExameRepository
    : ISolicitacaoExameRepository
{
    private readonly AppDbContext _context;

    public SolicitacaoExameRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<SolicitacaoExame>>
    GetByConsultaIdAsync(
        int consultaId
    )
    {
        return await _context
            .SolicitacoesExames
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .Where(x =>
                x.ConsultaId == consultaId
            )
            .OrderByDescending(x =>
                x.DataSolicitacao
            )
            .ToListAsync();
    }

    public async Task<List<SolicitacaoExame>> GetAllAsync()
    {
        return await _context.SolicitacoesExames
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                   .ThenInclude(m => m.User)
            .ToListAsync();
    }

    public async Task<SolicitacaoExame?> GetByIdAsync(
        int id
    )
    {
        return await _context.SolicitacoesExames
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                   .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(
                x => x.Id == id
            );
    }

    public async Task AddAsync(
        SolicitacaoExame solicitacao
    )
    {
        await _context.SolicitacoesExames
            .AddAsync(solicitacao);
    }

    public async Task DeleteAsync(
        SolicitacaoExame solicitacao
    )
    {
        _context.SolicitacoesExames
            .Remove(solicitacao);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}