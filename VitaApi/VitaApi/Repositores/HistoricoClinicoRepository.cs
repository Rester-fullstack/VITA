using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class HistoricoClinicoRepository
    : BaseRepository<HistoricoClinico>,
      IHistoricoClinicoRepository
{
    public HistoricoClinicoRepository(
        AppDbContext context
    ) : base(context)
    {
    }

    public async Task<List<HistoricoClinico>>
        GetAllAsync()
    {
        return await _context.HistoricosClinicos
            .Include(h => h.Paciente)
            .Include(h => h.Consulta)
            .ToListAsync();
    }

    public async Task<HistoricoClinico?>
        GetByIdAsync(int id)
    {
        return await _context.HistoricosClinicos
            .Include(h => h.Paciente)
            .Include(h => h.Consulta)
            .FirstOrDefaultAsync(
                h => h.Id == id
            );
    }
}