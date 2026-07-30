using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.DTOs.Dashboard;

namespace VitaApi.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }


    


    public async Task<DashboardAdminDto>
     GetAdminDashboardAsync()
    {
        var hoje = DateTime.UtcNow.Date;

        var inicioSemana =
            hoje.AddDays(-7);

        var ultimosPacientes =
            await _context.Pacientes
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => new
                {
                    x.Id,
                    x.Nome
                })
                .ToListAsync();

        var ultimosExames =
            await _context.Exames
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => new
                {
                    x.Id,
                    x.Nome,
                    x.Resultado
                })
                .ToListAsync();

        var timeline =
            await _context.Auditorias

                .OrderByDescending(x => x.DataHora)

                .Take(15)

                .Select(x => new DashboardTimelineDto
                {
                    Entidade = x.Entidade,
                    Acao = x.Acao,
                    Descricao = x.Descricao,
                    Usuario = x.UsuarioNome,
                    DataHora = x.DataHora,
                    Icone = x.Icone,
                    Cor = x.Cor
                })

                .ToListAsync();


        var consultasPorMes =
    await _context.Consultas
        .GroupBy(c => c.DataConsulta.Month)
        .Select(g => new DashboardChartDto
        {
            Nome = new DateTime(2000, g.Key, 1).ToString("MMM"),
            Valor = g.Count()
        })
        .ToListAsync();

        var documentosEmitidos =
            new List<DashboardChartDto>
            {
        new()
        {
            Nome = "Receitas",
            Valor = await _context.Receitas.CountAsync()
        },
        new()
        {
            Nome = "Atestados",
            Valor = await _context.Atestados.CountAsync()
        },
        new()
        {
            Nome = "Declarações",
            Valor = await _context.DeclaracoesComparecimento.CountAsync()
        },
        new()
        {
            Nome = "Solicitações",
            Valor = await _context.SolicitacoesExames.CountAsync()
        }
            };

        return new DashboardAdminDto
        {
            TotalPacientes =
        await _context.Pacientes.CountAsync(),

            TotalMedicos =
        await _context.Medicos.CountAsync(),

            TotalConsultas =
        await _context.Consultas.CountAsync(),

            TotalExames =
        await _context.Exames.CountAsync(),

            ConsultasHoje =
        await _context.Consultas
            .CountAsync(x =>
                x.DataConsulta.Date == hoje),

            ConsultasSemana =
        await _context.Consultas
            .CountAsync(x =>
                x.DataConsulta >= inicioSemana),

            ConsultasCanceladas =

        await _context.Consultas
              .CountAsync(x =>
                 x.Status == "Cancelada"),

            TotalReceitas =
        await _context.Receitas.CountAsync(),

            TotalAtestados =
        await _context.Atestados.CountAsync(),

            TotalSolicitacoesExames =
        await _context.SolicitacoesExames.CountAsync(),

            TotalDeclaracoes =
        await _context.DeclaracoesComparecimento.CountAsync(),

            Timeline =
        timeline,

            UltimosPacientes =
        ultimosPacientes,

            UltimosExames =
        ultimosExames,

            ConsultasPorMes = consultasPorMes,

            DocumentosEmitidos = documentosEmitidos,
        };
    }

    


    public async Task<DashboardMedicoDto> GetMedicoDashboardAsync(int medicoId)
    {
        var hoje = DateTime.UtcNow.Date;

        return new DashboardMedicoDto
        {
            TotalPacientes =
                await _context.Pacientes.CountAsync(),

            TotalConsultas =
                await _context.Consultas
                    .CountAsync(c =>
                        c.MedicoId == medicoId),

            ConsultasHoje =
                await _context.Consultas
                    .CountAsync(c =>
                        c.MedicoId == medicoId &&
                        c.DataConsulta.Date == hoje),

            TotalExames =
                await _context.Exames.CountAsync()
        };
    }
}