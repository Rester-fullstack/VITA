using VitaApi.DTOs.DeclaracoesComparecimento;

namespace VitaApi.Interfaces;

public interface IDeclaracaoComparecimentoService
{
    Task<List<DeclaracaoComparecimentoResponseDto>> GetAllAsync();

    Task<List<DeclaracaoComparecimentoResponseDto>> GetByConsultaIdAsync(
        int consultaId
    );

    Task<DeclaracaoComparecimentoResponseDto?> GetByIdAsync(
        int id
    );

    Task<DeclaracaoComparecimentoResponseDto> CreateAsync(
        CreateDeclaracaoComparecimentoDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    );

    Task<bool> DeleteAsync(
        int id
    );
}