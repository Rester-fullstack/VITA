using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
{
    public MedicoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Medico>> GetAllAsync()
    {
        return await _context.Medicos
            .Include(m => m.User)
            .Include(m => m.Especialidade)
            .ToListAsync();
    }

    public async Task<Medico?> GetByIdAsync(int id)
    {
        return await _context.Medicos
            .Include(m => m.User)
            .Include(m => m.Especialidade)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Medico?> GetByUserIdAsync(
    int userId
)
    {
        return await _context.Medicos
            .Include(m => m.User)
            .Include(m => m.Especialidade)
            .FirstOrDefaultAsync(
                m => m.UserId == userId
            );
    }
}