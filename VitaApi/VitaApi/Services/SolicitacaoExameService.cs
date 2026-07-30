using VitaApi.DTOs.SolicitacoesExames;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class SolicitacaoExameService
    : ISolicitacaoExameService
{
    private readonly ISolicitacaoExameRepository _repository;
    private readonly IAuditoriaService _auditoriaService;

    public SolicitacaoExameService(
        ISolicitacaoExameRepository repository,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _auditoriaService = auditoriaService;
    }

    public async Task<List<SolicitacaoExameResponseDto>> GetByConsultaIdAsync(
        int consultaId
    )
    {
        var lista =
            await _repository.GetByConsultaIdAsync(consultaId);

        return lista.Select(Map).ToList();
    }

    public async Task<List<SolicitacaoExameResponseDto>> GetAllAsync()
    {
        var lista =
            await _repository.GetAllAsync();

        return lista.Select(Map).ToList();
    }

    public async Task<SolicitacaoExameResponseDto?> GetByIdAsync(
        int id
    )
    {
        var solicitacao =
            await _repository.GetByIdAsync(id);

        if (solicitacao == null)
            return null;

        return Map(solicitacao);
    }

    public async Task<SolicitacaoExameResponseDto> CreateAsync(
        CreateSolicitacaoExameDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    )
    {
        var solicitacao =
            new SolicitacaoExame
            {
                ExamesSolicitados = dto.ExamesSolicitados,
                Justificativa = dto.Justificativa,
                ConsultaId = dto.ConsultaId,
                DataSolicitacao = DateTime.Now
            };

        await _repository.AddAsync(solicitacao);

        await _repository.SaveChangesAsync();

        solicitacao =
            await _repository.GetByIdAsync(
                solicitacao.Id
            ) ?? solicitacao;

        await _auditoriaService.RegistrarAsync(
            entidade: "Solicitação de Exame",
            acao: "Criou",
            descricao:
                $"Solicitação de exame emitida para {solicitacao.Consulta.Paciente.Nome}",
            consultaId: solicitacao.ConsultaId,
            pacienteId: solicitacao.Consulta.Paciente.Id,
            registroId: solicitacao.Id,
            icone: "🧪",
            cor: "#F59E0B"
        );

        return Map(solicitacao);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var solicitacao =
            await _repository.GetByIdAsync(id);

        if (solicitacao == null)
            return false;

        await _repository.DeleteAsync(solicitacao);

        await _repository.SaveChangesAsync();

        return true;
    }

    private static SolicitacaoExameResponseDto Map(
      SolicitacaoExame solicitacao
  )
    {
        return new SolicitacaoExameResponseDto
        {
            Id = solicitacao.Id,

            ExamesSolicitados =
                solicitacao.ExamesSolicitados,

            Justificativa =
                solicitacao.Justificativa,

            DataSolicitacao =
                solicitacao.DataSolicitacao,

            ConsultaId =
                solicitacao.ConsultaId,

            PacienteNome =
                solicitacao.Consulta.Paciente.Nome,

            MedicoNome =
                solicitacao.Consulta.Medico.User.Nome,

            MedicoCrm =
                solicitacao.Consulta.Medico.CRM,

            MedicoEspecialidade =
                solicitacao.Consulta.Medico.Especialidade?.Nome ?? "",

            MedicoTelefone =
                solicitacao.Consulta.Medico.Telefone ?? "",

            MedicoCidade =
                solicitacao.Consulta.Medico.Cidade ?? "",

            MedicoEstado =
                solicitacao.Consulta.Medico.Estado ?? "",

            MedicoEnderecoProfissional =
                solicitacao.Consulta.Medico.EnderecoProfissional ?? "",

            MedicoAssinatura =
                solicitacao.Consulta.Medico.Assinatura ?? ""
        };
    }
}