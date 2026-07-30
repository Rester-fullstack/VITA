using VitaApi.DTOs.Especialidades;

namespace VitaApi.Interfaces;

public interface IEspecialidadeService
{
    Task<List<EspecialidadeResponseDto>> GetAllAsync();

    Task<EspecialidadeResponseDto> CreateAsync(
    CreateEspecialidadeDto dto,
    int? usuarioId,
    string? usuarioNome,
    string? usuarioRole
);
}