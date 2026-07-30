using VitaApi.DTOs.Receitas;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class ReceitaService : IReceitaService
{
    private readonly IReceitaRepository _repository;
    private readonly IAuditoriaService _auditoriaService;

    public ReceitaService(
    IReceitaRepository repository,
    IAuditoriaService auditoriaService
)
    {
        _repository = repository;
        _auditoriaService = auditoriaService;
    }

    public async Task<List<ReceitaResponseDto>> GetAllAsync()
    {
        var receitas =
            await _repository.GetAllAsync();

        return receitas
            .Select(r => MapToResponse(r))
            .ToList();
    }

    public async Task<ReceitaResponseDto?> GetByIdAsync(
        int id
    )
    {
        var receita =
            await _repository.GetByIdAsync(id);

        if (receita == null)
            return null;

        return MapToResponse(receita);
    }

    public async Task<ReceitaResponseDto> CreateAsync(
        CreateReceitaDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    )
    {
        var receita =
            new Receita
            {
                Medicamento =
                    dto.Medicamento,

                Dosagem =
                    dto.Dosagem,

                Frequencia =
                    dto.Frequencia,

                Duracao =
                    dto.Duracao,

                Observacoes =
                    dto.Observacoes,

                ConsultaId =
                    dto.ConsultaId,

                PacienteId =
                    dto.PacienteId,

                DataReceita =
                    DateTime.UtcNow
            };

        await _repository.AddAsync(receita);

        await _repository.SaveChangesAsync();

        var receitaCompleta =
            await _repository.GetByIdAsync(
                receita.Id
            );

        await _auditoriaService.RegistrarAsync(

    entidade: "Receita",

    acao: "Criou",

    descricao:
        $"Receita emitida para {receitaCompleta!.Paciente?.Nome}",

    consultaId: receitaCompleta.ConsultaId,

    pacienteId: receitaCompleta.PacienteId,

    registroId: receitaCompleta.Id,

    icone: "💊",

    cor: "#22C55E"

);

        return MapToResponse(
            receitaCompleta!
        );
    }

    private ReceitaResponseDto MapToResponse(
        Receita receita
    )
    {
        return new ReceitaResponseDto
        {
            Id =
                receita.Id,

            Medicamento =
                receita.Medicamento,

            Dosagem =
                receita.Dosagem,

            Frequencia =
                receita.Frequencia,

            Duracao =
                receita.Duracao,

            Observacoes =
                receita.Observacoes,

            DataReceita =
                receita.DataReceita,

            ConsultaId =
                receita.ConsultaId,

            PacienteId =
                receita.PacienteId,

            PacienteNome =
                receita.Paciente?.Nome ?? "",

            MedicoNome =
                receita.Consulta?.Medico?.User?.Nome ?? "",

            MedicoCrm =
                receita.Consulta?.Medico?.CRM ?? ""
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}