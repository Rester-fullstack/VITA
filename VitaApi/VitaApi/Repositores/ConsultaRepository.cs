using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class ConsultaRepository
    : BaseRepository<Consulta>,
      IConsultaRepository
{
    public ConsultaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Consulta>> GetAllAsync()
    {
        return await _context.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
                .ThenInclude(m => m.User)
            .ToListAsync();
    }

    public async Task<Consulta?> GetByIdAsync(int id)
    {
        return await _context.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c => c.Id == id);

    }

    public async Task<Consulta?> GetByIdForDeleteAsync(int id)
    {
        return await _context.Consultas
            .Include(c => c.HistoricosClinicos)
            .Include(c => c.Receitas)
            .Include(c => c.Atestados)
            .Include(c => c.Exames)
            .Include(c => c.SolicitacoesExames)
            .Include(c => c.DeclaracoesComparecimento)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int?> GetMedicoIdByUserIdAsync(int userId)
    {
        return await _context.Medicos
            .Where(m => m.UserId == userId)
            .Select(m => (int?)m.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsConflictAsync(int medicoId, DateTime data)
    {
        return await _context.Consultas.AnyAsync(c =>
            c.MedicoId == medicoId &&
            c.DataConsulta >= data &&
            c.DataConsulta < data.AddMinutes(30) &&
            c.Status != "Cancelada"
        );
    }

    public async Task<List<Consulta>> GetByUserIdAsync(int userId)
    {
        return await _context.Consultas

            .Include(c => c.Paciente)

            .Include(c => c.Medico)
                .ThenInclude(m => m.User)

            .Where(c =>
                c.Medico.UserId == userId
            )

            .OrderBy(c =>
                c.DataConsulta
            )

            .ToListAsync();
    }


}