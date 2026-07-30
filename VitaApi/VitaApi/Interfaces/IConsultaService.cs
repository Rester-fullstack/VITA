using VitaApi.DTOs.Consultas;

namespace VitaApi.Interfaces;

public interface IConsultaService
{
    Task<List<ConsultaResponseDto>> GetAllAsync();

    Task<ConsultaResponseDto> CreateMinhaConsultaAsync(
        int userId,
        string? usuarioNome,
        string? usuarioRole,
        CreateMinhaConsultaDto dto
    );

    Task<List<ConsultaResponseDto>> GetMyConsultasAsync(int userId);

    Task<ConsultaResponseDto?> GetByIdAsync(int id);

    Task<ConsultaResponseDto> CreateAsync(
    CreateConsultaDto dto,
    int? usuarioId,
    string? usuarioNome,
    string? usuarioRole
);

    Task<bool> UpdateAsync(int id, UpdateConsultaDto dto);

    Task<bool> DeleteAsync(int id);
}