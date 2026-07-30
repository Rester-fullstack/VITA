using VitaApi.DTOs.Atestados;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class AtestadoService : IAtestadoService
{
    private readonly IAtestadoRepository _repository;
    private readonly IAuditoriaService _auditoriaService;

    public AtestadoService(
        IAtestadoRepository repository,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _auditoriaService = auditoriaService;
    }

    public async Task<List<AtestadoResponseDto>> GetAllAsync()
    {
        var atestados =
            await _repository.GetAllAsync();

        return atestados
            .Select(a => MapToResponse(a))
            .ToList();
    }

    public async Task<AtestadoResponseDto?> GetByIdAsync(int id)
    {
        var atestado =
            await _repository.GetByIdAsync(id);

        if (atestado == null)
            return null;

        return MapToResponse(atestado);
    }

    public async Task<AtestadoResponseDto> CreateAsync(
        CreateAtestadoDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    )
    {
        var atestado =
            new Atestado
            {
                Motivo = dto.Motivo,
                Cid = dto.Cid,
                DataInicio = dto.DataInicio,
                DiasAfastamento = dto.DiasAfastamento,
                Observacoes = dto.Observacoes,
                ConsultaId = dto.ConsultaId,
                PacienteId = dto.PacienteId,
                DataEmissao = DateTime.UtcNow
            };

        await _repository.AddAsync(atestado);

        await _repository.SaveChangesAsync();

        var atestadoCompleto =
            await _repository.GetByIdAsync(atestado.Id);

        await _auditoriaService.RegistrarAsync(
            entidade: "Atestado",
            acao: "Criou",
            descricao:
                $"Atestado emitido para {atestadoCompleto!.Paciente?.Nome}",
            consultaId: atestadoCompleto.ConsultaId,
            pacienteId: atestadoCompleto.PacienteId,
            registroId: atestadoCompleto.Id,
            icone: "📄",
            cor: "#EF4444"
        );

        return MapToResponse(atestadoCompleto);
    }

    private AtestadoResponseDto MapToResponse(
     Atestado atestado
 )
    {
        return new AtestadoResponseDto
        {
            Id = atestado.Id,

            Motivo = atestado.Motivo,

            Cid = atestado.Cid,

            DataInicio = atestado.DataInicio,

            DiasAfastamento = atestado.DiasAfastamento,

            Observacoes = atestado.Observacoes,

            DataEmissao = atestado.DataEmissao,

            ConsultaId = atestado.ConsultaId,

            PacienteId = atestado.PacienteId,

            PacienteNome =
                atestado.Paciente?.Nome ?? "",

            MedicoNome =
                atestado.Consulta?.Medico?.User?.Nome ?? "",

            MedicoCrm =
                atestado.Consulta?.Medico?.CRM ?? "",

            MedicoEspecialidade =
                atestado.Consulta?.Medico?.Especialidade?.Nome ?? "",

            MedicoTelefone =
                atestado.Consulta?.Medico?.Telefone ?? "",

            MedicoCidade =
                atestado.Consulta?.Medico?.Cidade ?? "",

            MedicoEstado =
                atestado.Consulta?.Medico?.Estado ?? "",

            MedicoEnderecoProfissional =
                atestado.Consulta?.Medico?.EnderecoProfissional ?? "",

            MedicoAssinatura =
                atestado.Consulta?.Medico?.Assinatura ?? ""
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}