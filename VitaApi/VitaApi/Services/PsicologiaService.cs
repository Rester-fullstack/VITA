using VitaApi.DTOs.Psicologia;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class PsicologiaService : IPsicologiaService
{
    private readonly IPsicologiaRepository _repository;

    public PsicologiaService(
        IPsicologiaRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<List<PsicologiaRegistroResponseDto>> GetAllAsync()
    {
        var registros =
            await _repository.GetAllAsync();

        return registros
            .Select(r => MapToResponse(r))
            .ToList();
    }

    public async Task<PsicologiaRegistroResponseDto?> GetByIdAsync(
        int id
    )
    {
        var registro =
            await _repository.GetByIdAsync(id);

        if (registro == null)
            return null;

        return MapToResponse(registro);
    }

    public async Task<PsicologiaRegistroResponseDto> CreateAsync(
        CreatePsicologiaRegistroDto dto
    )
    {
        var registro =
            new PsicologiaRegistro
            {
                Humor =
                    dto.Humor,

                QueixaPrincipal =
                    dto.QueixaPrincipal,

                EvolucaoSessao =
                    dto.EvolucaoSessao,

                Observacoes =
                    dto.Observacoes,

                ConsultaId =
                    dto.ConsultaId,

                PacienteId =
                    dto.PacienteId,

                DataRegistro =
                    DateTime.UtcNow
            };

        await _repository.AddAsync(registro);

        await _repository.SaveChangesAsync();

        var completo =
            await _repository.GetByIdAsync(
                registro.Id
            );

        return MapToResponse(completo!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private PsicologiaRegistroResponseDto MapToResponse(
        PsicologiaRegistro registro
    )
    {
        return new PsicologiaRegistroResponseDto
        {
            Id =
                registro.Id,

            Humor =
                registro.Humor,

            QueixaPrincipal =
                registro.QueixaPrincipal,

            EvolucaoSessao =
                registro.EvolucaoSessao,

            Observacoes =
                registro.Observacoes,

            DataRegistro =
                registro.DataRegistro,

            ConsultaId =
                registro.ConsultaId,

            PacienteId =
                registro.PacienteId,

            PacienteNome =
                registro.Paciente?.Nome ?? ""
        };
    }
}