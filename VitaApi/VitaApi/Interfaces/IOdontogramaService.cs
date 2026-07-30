using VitaApi.DTOs.Odontogramas;

namespace VitaApi.Interfaces;

public interface IOdontogramaService
{
    Task<List<OdontogramaResponseDto>> GetAllAsync();

    Task<OdontogramaResponseDto?> GetByIdAsync(int id);

    Task<OdontogramaResponseDto> CreateAsync(
        CreateOdontogramaDto dto
    );

    Task<bool> DeleteAsync(int id);
}