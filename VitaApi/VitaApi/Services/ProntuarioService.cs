using VitaApi.DTOs.Prontuarios;
using VitaApi.Interfaces;

namespace VitaApi.Services;

public class ProntuarioService
    : IProntuarioService
{
    private readonly IProntuarioRepository _repository;

    public ProntuarioService(
        IProntuarioRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<ProntuarioPacienteDto?> GetPacienteAsync(
        int pacienteId
    )
    {
        var paciente =
            await _repository.GetPacienteCompletoAsync(
                pacienteId
            );

        if (paciente == null)
            return null;

        return new ProntuarioPacienteDto
        {
            PacienteId = paciente.Id,

            PacienteNome = paciente.Nome,

            CPF = paciente.CPF,

            Telefone = paciente.Telefone,

            DataNascimento = paciente.DataNascimento,

            Consultas =
                paciente.Consultas
                    .OrderByDescending(c =>
                        c.DataConsulta
                    )
                    .Select(c =>
                        new ProntuarioConsultaDto
                        {
                            ConsultaId = c.Id,

                            DataConsulta =
                                c.DataConsulta,

                            Status =
                                c.Status,

                            Observacoes =
                                c.Observacoes,

                            MedicoNome =
                                c.Medico.User.Nome,

                            MedicoCrm =
                                c.Medico.CRM,

                            MedicoEspecialidade =
                                c.Medico.Especialidade?.Nome ?? "",

                            MedicoTelefone =
                                c.Medico.Telefone ?? "",

                            MedicoCidade =
                                c.Medico.Cidade ?? "",

                            MedicoEstado =
                                c.Medico.Estado ?? "",

                            MedicoEnderecoProfissional =
                                c.Medico.EnderecoProfissional ?? "",
 
                            MedicoAssinatura =
                               c.Medico.Assinatura ?? "",

                            Historicos =
                                c.HistoricosClinicos
                                    .OrderByDescending(h =>
                                        h.DataRegistro
                                    )
                                    .Select(h =>
                                        h.Descricao
                                    )
                                    .ToList(),

                            Receitas =
                                c.Receitas
                                    .OrderByDescending(r =>
                                        r.DataReceita
                                    )
                                    .Select(r =>
                                        $"{r.Medicamento} • {r.Dosagem} • {r.Frequencia} • {r.Duracao}"
                                    )
                                    .ToList(),

                            Atestados =
                                c.Atestados
                                    .OrderByDescending(a =>
                                        a.DataEmissao
                                    )
                                    .Select(a =>
                                        $"{a.Motivo} • {a.DiasAfastamento} dia(s)"
                                    )
                                    .ToList(),

                            Declaracoes =
                                c.DeclaracoesComparecimento
                                    .OrderByDescending(d =>
                                        d.DataEmissao
                                    )
                                    .Select(d =>
                                        d.Observacoes ??
                                        "Declaração de comparecimento emitida"
                                    )
                                    .ToList(),

                            SolicitacoesExames =
                                c.SolicitacoesExames
                                    .OrderByDescending(s =>
                                        s.DataSolicitacao
                                    )
                                    .Select(s =>
                                        s.ExamesSolicitados
                                    )
                                    .ToList(),

                            Exames =
                                c.Exames
                                    .OrderByDescending(e =>
                                        e.DataExame
                                    )
                                    .Select(e =>
                                        $"{e.Nome} • {e.Resultado}"
                                    )
                                    .ToList()
                        }
                    )
                    .ToList()
        };
    }
}