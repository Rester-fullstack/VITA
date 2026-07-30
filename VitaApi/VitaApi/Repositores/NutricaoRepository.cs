using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class NutricaoRepository
    : INutricaoRepository
{
    private readonly AppDbContext _context;

    public NutricaoRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<NutricaoRegistro>> GetAllAsync()
    {
        return await _context.NutricaoRegistros
            .Include(x => x.Paciente)
            .Include(x => x.Consulta)
            .ToListAsync();
    }

    public async Task<NutricaoRegistro?> GetByIdAsync(int id)
    {
        return await _context.NutricaoRegistros
            .Include(x => x.Paciente)
            .Include(x => x.Consulta)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        NutricaoRegistro registro
    )
    {
        await _context.NutricaoRegistros
            .AddAsync(registro);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var registro =
            await _context.NutricaoRegistros
                .FirstOrDefaultAsync(x => x.Id == id);

        if (registro == null)
            return false;

        _context.NutricaoRegistros.Remove(registro);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}