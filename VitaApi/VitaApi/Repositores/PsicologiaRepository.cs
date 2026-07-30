using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class PsicologiaRepository
    : IPsicologiaRepository
{
    private readonly AppDbContext _context;

    public PsicologiaRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<PsicologiaRegistro>> GetAllAsync()
    {
        return await _context.PsicologiaRegistros
            .Include(x => x.Paciente)
            .Include(x => x.Consulta)
            .ToListAsync();
    }

    public async Task<PsicologiaRegistro?> GetByIdAsync(
        int id
    )
    {
        return await _context.PsicologiaRegistros
            .Include(x => x.Paciente)
            .Include(x => x.Consulta)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        PsicologiaRegistro registro
    )
    {
        await _context.PsicologiaRegistros
            .AddAsync(registro);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var registro =
            await _context.PsicologiaRegistros
                .FirstOrDefaultAsync(x => x.Id == id);

        if (registro == null)
            return false;

        _context.PsicologiaRegistros.Remove(registro);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}