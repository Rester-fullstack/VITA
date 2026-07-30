using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.DTOs.ConfiguracoesClinica;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Medico")]
public class ConfiguracaoClinicaController
    : ControllerBase
{
    private readonly IConfiguracaoClinicaService _service;

    public ConfiguracaoClinicaController(
        IConfiguracaoClinicaService service
    )
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var config =
            await _service.GetAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Configuração carregada.",
                Data = config
            });
    }


    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update(
        UpdateConfiguracaoClinicaDto dto
    )
    {
        var config =
            await _service.UpdateAsync(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Configuração salva com sucesso.",
                Data = config
            });
    }
}