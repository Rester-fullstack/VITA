using VitaApi.DTOs.DeclaracoesComparecimento;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class DeclaracaoComparecimentoService
    : IDeclaracaoComparecimentoService
{
    private readonly IDeclaracaoComparecimentoRepository _repository;
    private readonly IAuditoriaService _auditoriaService;

    public DeclaracaoComparecimentoService(
        IDeclaracaoComparecimentoRepository repository,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _auditoriaService = auditoriaService;
    }

    public async Task<List<DeclaracaoComparecimentoResponseDto>> GetAllAsync()
    {
        var lista =
            await _repository.GetAllAsync();

        return lista.Select(Map).ToList();
    }

    public async Task<List<DeclaracaoComparecimentoResponseDto>> GetByConsultaIdAsync(
        int consultaId
    )
    {
        var lista =
            await _repository.GetByConsultaIdAsync(
                consultaId
            );

        return lista.Select(Map).ToList();
    }

    public async Task<DeclaracaoComparecimentoResponseDto?> GetByIdAsync(
        int id
    )
    {
        var declaracao =
            await _repository.GetByIdAsync(id);

        if (declaracao == null)
            return null;

        return Map(declaracao);
    }

    public async Task<DeclaracaoComparecimentoResponseDto> CreateAsync(
        CreateDeclaracaoComparecimentoDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    )
    {
        var declaracao =
            new DeclaracaoComparecimento
            {
                ConsultaId = dto.ConsultaId,
                Observacoes = dto.Observacoes,
                DataEmissao = DateTime.Now
            };

        await _repository.AddAsync(declaracao);

        await _repository.SaveChangesAsync();

        declaracao =
            await _repository.GetByIdAsync(
                declaracao.Id
            ) ?? declaracao;

        await _auditoriaService.RegistrarAsync(
            entidade: "Declaração",
            acao: "Criou",
            descricao:
                $"Declaração emitida para {declaracao.Consulta.Paciente.Nome}",
            consultaId: declaracao.ConsultaId,
            pacienteId: declaracao.Consulta.Paciente.Id,
            registroId: declaracao.Id,
            icone: "🧾",
            cor: "#0EA5E9"
        );

        return Map(declaracao);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var declaracao =
            await _repository.GetByIdAsync(id);

        if (declaracao == null)
            return false;

        await _repository.DeleteAsync(declaracao);

        await _repository.SaveChangesAsync();

        return true;
    }

    private static DeclaracaoComparecimentoResponseDto Map(
     DeclaracaoComparecimento declaracao
 )
    {
        return new DeclaracaoComparecimentoResponseDto
        {
            Id = declaracao.Id,

            ConsultaId = declaracao.ConsultaId,

            Observacoes = declaracao.Observacoes,

            DataEmissao = declaracao.DataEmissao,

            DataConsulta = declaracao.Consulta.DataConsulta,

            PacienteNome =
                declaracao.Consulta.Paciente.Nome,

            MedicoNome =
                declaracao.Consulta.Medico.User.Nome,

            MedicoCrm =
                declaracao.Consulta.Medico.CRM,

            MedicoEspecialidade =
                declaracao.Consulta.Medico.Especialidade?.Nome ?? "",

            MedicoTelefone =
                declaracao.Consulta.Medico.Telefone ?? "",

            MedicoCidade =
                declaracao.Consulta.Medico.Cidade ?? "",

            MedicoEstado =
                declaracao.Consulta.Medico.Estado ?? "",

            MedicoEnderecoProfissional =
                declaracao.Consulta.Medico.EnderecoProfissional ?? "",

            MedicoAssinatura =
                declaracao.Consulta.Medico.Assinatura ?? ""
        };
    }
}