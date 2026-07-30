using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class ProntuarioRepository
    : IProntuarioRepository
{
    private readonly AppDbContext _context;

    public ProntuarioRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<Paciente?> GetPacienteCompletoAsync(
        int pacienteId
    )
    {
        return await _context.Pacientes
            .Include(p => p.Consultas)
                .ThenInclude(c => c.Medico)
                    .ThenInclude(m => m.User)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.HistoricosClinicos)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.Exames)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.Receitas)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.Atestados)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.DeclaracoesComparecimento)

            .Include(p => p.Consultas)
                .ThenInclude(c => c.SolicitacoesExames)

            .FirstOrDefaultAsync(p =>
                p.Id == pacienteId
            );
    }
}