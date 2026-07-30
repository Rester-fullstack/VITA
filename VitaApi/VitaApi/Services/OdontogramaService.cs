using VitaApi.DTOs.Odontogramas;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class OdontogramaService : IOdontogramaService
{
    private readonly IOdontogramaRepository _repository;

    public OdontogramaService(
        IOdontogramaRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<List<OdontogramaResponseDto>> GetAllAsync()
    {
        var odontogramas =
            await _repository.GetAllAsync();

        return odontogramas
            .Select(o => MapToResponse(o))
            .ToList();
    }

    public async Task<OdontogramaResponseDto?> GetByIdAsync(
        int id
    )
    {
        var odontograma =
            await _repository.GetByIdAsync(id);

        if (odontograma == null)
            return null;

        return MapToResponse(odontograma);
    }

    public async Task<OdontogramaResponseDto> CreateAsync(
        CreateOdontogramaDto dto
    )
    {
        var odontograma =
            new Odontograma
            {
                Dente = dto.Dente,
                Status = dto.Status,
                Observacoes = dto.Observacoes,
                ConsultaId = dto.ConsultaId,
                PacienteId = dto.PacienteId,
                DataRegistro = DateTime.UtcNow
            };

        await _repository.AddAsync(odontograma);

        await _repository.SaveChangesAsync();

        var completo =
            await _repository.GetByIdAsync(
                odontograma.Id
            );

        return MapToResponse(completo!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private OdontogramaResponseDto MapToResponse(
        Odontograma odontograma
    )
    {
        return new OdontogramaResponseDto
        {
            Id = odontograma.Id,
            Dente = odontograma.Dente,
            Status = odontograma.Status,
            Observacoes = odontograma.Observacoes,
            DataRegistro = odontograma.DataRegistro,
            ConsultaId = odontograma.ConsultaId,
            PacienteId = odontograma.PacienteId,
            PacienteNome =
                odontograma.Paciente?.Nome ?? ""
        };
    }
}