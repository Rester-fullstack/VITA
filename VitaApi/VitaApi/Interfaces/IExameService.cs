using VitaApi.DTOs.Exames;
using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IExameService
{
    Task<List<ExameResponseDto>>
        GetAllAsync();

    Task<ExameResponseDto?>
        GetByIdAsync(int id);
    Task<ExameResponseDto> CreateAsync(
        CreateExameDto dto,
        int? usuarioId,
        string? usuarioNome,
        string? usuarioRole
    );

    Task<bool>
        UpdateAsync(
            int id,
            UpdateExameDto dto
        );

    Task<bool>
        DeleteAsync(int id);

   

    Task<Exame?>
        GetEntityByIdAsync(int id);

    Task
        SaveChangesAsync();
}