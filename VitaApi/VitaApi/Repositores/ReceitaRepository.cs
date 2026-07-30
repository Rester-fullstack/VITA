using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class ReceitaRepository : IReceitaRepository
{
    private readonly AppDbContext _context;

    public ReceitaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Receita>> GetAllAsync()
    {
        return await _context.Receitas
         .Include(r => r.Paciente)
         .Include(r => r.Consulta)
             .ThenInclude(c => c.Medico)
                 .ThenInclude(m => m.User)
         .ToListAsync();
    }

    public async Task<Receita?> GetByIdAsync(int id)
    {
        return await _context.Receitas
            .Include(r => r.Paciente)
            .Include(r => r.Consulta)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Receita receita)
    {
        await _context.Receitas.AddAsync(receita);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var receita =
            await _context.Receitas
                .FirstOrDefaultAsync(x => x.Id == id);

        if (receita == null)
            return false;

        _context.Receitas.Remove(receita);

        await _context.SaveChangesAsync();

        return true;
    }
}