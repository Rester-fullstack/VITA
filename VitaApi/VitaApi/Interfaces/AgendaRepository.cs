using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

public class AgendaRepository : IAgendaRepository
{
    private readonly AppDbContext _context;

    public AgendaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AgendaMedica>> GetByMedicoAsync(int medicoId)
    {
        return await _context.AgendaMedica
            .Include(x => x.Consulta)
            .Where(x => x.MedicoId == medicoId)
            .OrderBy(x => x.DataHora)
            .ToListAsync();
    }

    public async Task<List<Consulta>> GetByUserIdAsync(int userId)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
                .ThenInclude(m => m.User)
            .Where(c => c.Medico.UserId == userId)
            .OrderBy(c => c.DataConsulta)
            .ToListAsync();
    }

    public async Task AddAsync(AgendaMedica agenda)
    {
        await _context.AgendaMedica.AddAsync(agenda);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}