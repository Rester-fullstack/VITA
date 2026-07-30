using VitaApi.DTOs.Psicologia;

namespace VitaApi.Interfaces;

public interface IPsicologiaService
{
    Task<List<PsicologiaRegistroResponseDto>> GetAllAsync();

    Task<PsicologiaRegistroResponseDto?> GetByIdAsync(int id);

    Task<PsicologiaRegistroResponseDto> CreateAsync(
        CreatePsicologiaRegistroDto dto
    );

    Task<bool> DeleteAsync(int id);
}