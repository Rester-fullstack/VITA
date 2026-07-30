using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class EspecialidadeRepository
    : BaseRepository<Especialidade>,
      IEspecialidadeRepository
{
    public EspecialidadeRepository(
        AppDbContext context
    ) : base(context)
    {
    }

    public async Task<List<Especialidade>>
        GetAllAsync()
    {
        return await _context.Especialidades
            .Include(e => e.Medicos)
            .ToListAsync();
    }

    public async Task<Especialidade?>
        GetByIdAsync(int id)
    {
        return await _context.Especialidades
            .Include(e => e.Medicos)
            .FirstOrDefaultAsync(
                e => e.Id == id
            );
    }
}