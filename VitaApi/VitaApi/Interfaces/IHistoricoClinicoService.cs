using VitaApi.DTOs.Historicos;

namespace VitaApi.Interfaces;

public interface IHistoricoClinicoService
{
    Task<List<HistoricoResponseDto>>
        GetAllAsync();

    Task<HistoricoResponseDto?>
        GetByIdAsync(int id);

    Task<HistoricoResponseDto>
        CreateAsync(CreateHistoricoDto dto);

    Task<bool>
        UpdateAsync(
            int id,
            UpdateHistoricoDto dto
        );

    Task<bool>
        DeleteAsync(int id);
}