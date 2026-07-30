using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class PacienteRepository
    : BaseRepository<Paciente>,
      IPacienteRepository
{
    public PacienteRepository(
        AppDbContext context
    ) : base(context)
    {
    }

    public async Task<List<Paciente>>
        GetAllAsync()
    {
        return await _context.Pacientes
            .ToListAsync();
    }

    public async Task<List<Paciente>>
        GetPagedAsync(
            int page,
            int pageSize
        )
    {
        return await _context.Pacientes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Paciente?>
        GetByIdAsync(int id)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(
                x => x.Id == id
            );
    }
}