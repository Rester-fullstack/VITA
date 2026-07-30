using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class DeclaracaoComparecimentoRepository
    : IDeclaracaoComparecimentoRepository
{
    private readonly AppDbContext _context;

    public DeclaracaoComparecimentoRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<DeclaracaoComparecimento>> GetAllAsync()
    {
        return await _context.DeclaracoesComparecimento
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .OrderByDescending(x => x.DataEmissao)
            .ToListAsync();
    }

    public async Task<List<DeclaracaoComparecimento>> GetByConsultaIdAsync(
        int consultaId
    )
    {
        return await _context.DeclaracoesComparecimento
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .Where(x => x.ConsultaId == consultaId)
            .OrderByDescending(x => x.DataEmissao)
            .ToListAsync();
    }

    public async Task<DeclaracaoComparecimento?> GetByIdAsync(
        int id
    )
    {
        return await _context.DeclaracoesComparecimento
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Paciente)
            .Include(x => x.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        DeclaracaoComparecimento declaracao
    )
    {
        await _context.DeclaracoesComparecimento
            .AddAsync(declaracao);
    }

    public async Task DeleteAsync(
        DeclaracaoComparecimento declaracao
    )
    {
        _context.DeclaracoesComparecimento
            .Remove(declaracao);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}