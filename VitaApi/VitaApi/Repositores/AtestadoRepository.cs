using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class AtestadoRepository : IAtestadoRepository
{
    private readonly AppDbContext _context;

    public AtestadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Atestado>> GetAllAsync()
    {
        return await _context.Atestados
            .Include(a => a.Paciente)
            .Include(a => a.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .ToListAsync();
    }

    public async Task<Atestado?> GetByIdAsync(int id)
    {
        return await _context.Atestados
            .Include(a => a.Paciente)
            .Include(a => a.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Atestado atestado)
    {
        await _context.Atestados.AddAsync(atestado);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var atestado =
            await _context.Atestados
                .FirstOrDefaultAsync(x => x.Id == id);

        if (atestado == null)
            return false;

        _context.Atestados.Remove(atestado);

        await _context.SaveChangesAsync();

        return true;
    }
}