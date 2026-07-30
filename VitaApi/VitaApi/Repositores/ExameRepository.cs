using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class ExameRepository : IExameRepository
{
    private readonly AppDbContext _context;

    public ExameRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<Exame>>
        GetAllAsync()
    {
        return await _context.Exames
            .Include(e => e.Paciente)
            .Include(e => e.Consulta)
            .ToListAsync();
    }

    public async Task<Exame?>
        GetByIdAsync(int id)
    {
        return await _context.Exames
            .Include(e => e.Paciente)
            .Include(e => e.Consulta)
            .FirstOrDefaultAsync(
                e => e.Id == id
            );
    }

    public async Task AddAsync(
        Exame exame
    )
    {
        await _context.Exames.AddAsync(
            exame
        );
    }

    public async Task UpdateAsync(
        Exame exame
    )
    {
        _context.Exames.Update(exame);

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(
        Exame exame
    )
    {
        _context.Exames.Remove(exame);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}