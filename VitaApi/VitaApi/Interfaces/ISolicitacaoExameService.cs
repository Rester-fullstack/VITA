using VitaApi.DTOs.SolicitacoesExames;

namespace VitaApi.Interfaces;

public interface ISolicitacaoExameService
{
    Task<List<SolicitacaoExameResponseDto>> GetAllAsync();

    Task<List<SolicitacaoExameResponseDto>>
    GetByConsultaIdAsync(
        int consultaId
    );

    Task<SolicitacaoExameResponseDto?> GetByIdAsync(int id);

    Task<SolicitacaoExameResponseDto> CreateAsync(
        CreateSolicitacaoExameDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    );

    Task<bool> DeleteAsync(int id);
}