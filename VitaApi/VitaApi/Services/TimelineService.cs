using Microsoft.EntityFrameworkCore;
using VitaApi.Data;
using VitaApi.DTOs.Timeline;
using VitaApi.Interfaces;

namespace VitaApi.Services;

public class TimelineService : ITimelineService
{
    private readonly AppDbContext _context;

    public TimelineService(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<TimelineItemDto>> GetTimelinePacienteAsync(
        int pacienteId
    )
    {
        var timeline =
            new List<TimelineItemDto>();

        var consultas =
            await _context.Consultas
                .Include(c => c.Medico)
                    .ThenInclude(m => m.User)
                .Include(c => c.HistoricosClinicos)
                .Include(c => c.Receitas)
                .Include(c => c.Atestados)
                .Include(c => c.DeclaracoesComparecimento)
                .Include(c => c.SolicitacoesExames)
                .Include(c => c.Exames)
                .Where(c =>
                    c.PacienteId == pacienteId
                )
                .ToListAsync();

        foreach (var consulta in consultas)
        {
            timeline.Add(new TimelineItemDto
            {
                Data = consulta.DataConsulta,
                Tipo = "Consulta",
                Titulo = "Consulta médica",
                Descricao =
                    $"{consulta.Medico.User.Nome} • {consulta.Status}",
                ConsultaId = consulta.Id
            });

            foreach (var historico in consulta.HistoricosClinicos)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = historico.DataRegistro,
                    Tipo = "Histórico",
                    Titulo = "Evolução clínica",
                    Descricao = historico.Descricao,
                    ConsultaId = consulta.Id,
                    DocumentoId = historico.Id
                });
            }

            foreach (var receita in consulta.Receitas)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = receita.DataReceita,
                    Tipo = "Receita",
                    Titulo = "Receita emitida",
                    Descricao =
                        $"{receita.Medicamento} • {receita.Dosagem}",
                    ConsultaId = consulta.Id,
                    DocumentoId = receita.Id
                });
            }

            foreach (var atestado in consulta.Atestados)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = atestado.DataEmissao,
                    Tipo = "Atestado",
                    Titulo = "Atestado emitido",
                    Descricao =
                        $"{atestado.Motivo} • {atestado.DiasAfastamento} dia(s)",
                    ConsultaId = consulta.Id,
                    DocumentoId = atestado.Id
                });
            }

            foreach (var declaracao in consulta.DeclaracoesComparecimento)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = declaracao.DataEmissao,
                    Tipo = "Declaração",
                    Titulo = "Declaração de comparecimento",
                    Descricao =
                        declaracao.Observacoes ??
                        "Declaração emitida",
                    ConsultaId = consulta.Id,
                    DocumentoId = declaracao.Id
                });
            }

            foreach (var solicitacao in consulta.SolicitacoesExames)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = solicitacao.DataSolicitacao,
                    Tipo = "Solicitação",
                    Titulo = "Solicitação de exames",
                    Descricao = solicitacao.ExamesSolicitados,
                    ConsultaId = consulta.Id,
                    DocumentoId = solicitacao.Id
                });
            }

            foreach (var exame in consulta.Exames)
            {
                timeline.Add(new TimelineItemDto
                {
                    Data = exame.DataExame,
                    Tipo = "Exame",
                    Titulo = exame.Nome,
                    Descricao = exame.Resultado,
                    ConsultaId = consulta.Id,
                    DocumentoId = exame.Id
                });
            }
        }

        return timeline
            .OrderByDescending(x => x.Data)
            .ToList();
    }
}