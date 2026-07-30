using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class OdontogramaRepository : IOdontogramaRepository
{
    private readonly AppDbContext _context;

    public OdontogramaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Odontograma>> GetAllAsync()
    {
        return await _context.Odontogramas
            .Include(o => o.Paciente)
            .Include(o => o.Consulta)
            .ToListAsync();
    }

    public async Task<Odontograma?> GetByIdAsync(int id)
    {
        return await _context.Odontogramas
            .Include(o => o.Paciente)
            .Include(o => o.Consulta)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Odontograma odontograma)
    {
        await _context.Odontogramas.AddAsync(odontograma);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var odontograma =
            await _context.Odontogramas
                .FirstOrDefaultAsync(o => o.Id == id);

        if (odontograma == null)
            return false;

        _context.Odontogramas.Remove(odontograma);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}