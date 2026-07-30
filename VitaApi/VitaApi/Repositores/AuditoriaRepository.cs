using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly AppDbContext _context;

    public AuditoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Auditoria>> GetAllAsync()
    {
        return await _context.Auditorias
            .OrderByDescending(x => x.DataHora)
            .ToListAsync();
    }

    public async Task AddAsync(Auditoria auditoria)
    {
        await _context.Auditorias.AddAsync(auditoria);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}