using VitaApi.DTOs.ConfiguracoesClinica;

namespace VitaApi.Interfaces;

public interface IConfiguracaoClinicaService
{
    Task<ConfiguracaoClinicaDto> GetAsync();

    Task<ConfiguracaoClinicaDto> UpdateAsync(
        UpdateConfiguracaoClinicaDto dto
    );
}