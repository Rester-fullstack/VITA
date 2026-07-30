using VitaApi.DTOs.Nutricao;

namespace VitaApi.Interfaces;

public interface INutricaoService
{
    Task<List<NutricaoRegistroResponseDto>> GetAllAsync();

    Task<NutricaoRegistroResponseDto?> GetByIdAsync(int id);

    Task<NutricaoRegistroResponseDto> CreateAsync(
        CreateNutricaoRegistroDto dto
    );

    Task<bool> DeleteAsync(int id);
}