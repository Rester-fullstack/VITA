using VitaApi.DTOs.Nutricao;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class NutricaoService : INutricaoService
{
    private readonly INutricaoRepository _repository;

    public NutricaoService(
        INutricaoRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<List<NutricaoRegistroResponseDto>> GetAllAsync()
    {
        var registros =
            await _repository.GetAllAsync();

        return registros
            .Select(r => MapToResponse(r))
            .ToList();
    }

    public async Task<NutricaoRegistroResponseDto?> GetByIdAsync(
        int id
    )
    {
        var registro =
            await _repository.GetByIdAsync(id);

        if (registro == null)
            return null;

        return MapToResponse(registro);
    }

    public async Task<NutricaoRegistroResponseDto> CreateAsync(
        CreateNutricaoRegistroDto dto
    )
    {
        var imc =
            dto.Altura > 0
                ? dto.Peso / (dto.Altura * dto.Altura)
                : 0;

        var registro =
            new NutricaoRegistro
            {
                Peso =
                    dto.Peso,

                Altura =
                    dto.Altura,

                Imc =
                    Math.Round(imc, 2),

                Objetivo =
                    dto.Objetivo,

                PlanoAlimentar =
                    dto.PlanoAlimentar,

                Evolucao =
                    dto.Evolucao,

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

    private NutricaoRegistroResponseDto MapToResponse(
        NutricaoRegistro registro
    )
    {
        return new NutricaoRegistroResponseDto
        {
            Id =
                registro.Id,

            Peso =
                registro.Peso,

            Altura =
                registro.Altura,

            Imc =
                registro.Imc,

            Objetivo =
                registro.Objetivo,

            PlanoAlimentar =
                registro.PlanoAlimentar,

            Evolucao =
                registro.Evolucao,

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