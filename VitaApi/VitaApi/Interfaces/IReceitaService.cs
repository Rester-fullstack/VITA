using VitaApi.DTOs.Receitas;

namespace VitaApi.Interfaces;

public interface IReceitaService
{
    Task<List<ReceitaResponseDto>> GetAllAsync();

    Task<ReceitaResponseDto?> GetByIdAsync(int id);

    Task<ReceitaResponseDto> CreateAsync(

        CreateReceitaDto dto,

        int? usuarioId,

        string? usuarioNome,

        string? usuarioRole

    );

    Task<bool> DeleteAsync(int id);
}