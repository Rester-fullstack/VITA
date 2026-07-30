using VitaApi.DTOs.Atestados;

namespace VitaApi.Interfaces;

public interface IAtestadoService
{
    Task<List<AtestadoResponseDto>> GetAllAsync();

    Task<AtestadoResponseDto?> GetByIdAsync(int id);

    Task<AtestadoResponseDto> CreateAsync(
        CreateAtestadoDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    );
    Task<bool> DeleteAsync(int id);
}