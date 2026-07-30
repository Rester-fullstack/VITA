using VitaApi.DTOs.Pacientes;

namespace VitaApi.Interfaces;

public interface IPacienteService
{
    Task<List<PacienteResponseDto>> GetAllAsync();

    Task<List<PacienteResponseDto>>
    GetPagedAsync(
        int page,
        int pageSize
    );

    Task<PacienteResponseDto?> GetByIdAsync(int id);

    Task<PacienteResponseDto> CreateAsync(
        CreatePacienteDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    );

    Task<bool> UpdateAsync(int id, UpdatePacienteDto dto);

    Task<bool> DeleteAsync(int id);
}