using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Historicos;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoricoClinicoController
    : ControllerBase
{
    private readonly IHistoricoClinicoService _service;
    private readonly IConsultaService _consultaService;

    public HistoricoClinicoController(
        IHistoricoClinicoService service,
        IConsultaService consultaService
    )
    {
        _service = service;
        _consultaService = consultaService;
    }

    private async Task<bool> MedicoPodeAcessarConsulta(
        int consultaId
    )
    {
        var role =
            User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
            return true;

        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            );

        if (
            role != "Medico" ||
            userIdClaim == null
        )
            return false;

        var userId =
            int.Parse(userIdClaim.Value);

        var minhasConsultas =
            await _consultaService
                .GetMyConsultasAsync(userId);

        return minhasConsultas.Any(c =>
            c.Id == consultaId
        );
    }

    

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var historicos =
            await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Históricos encontrados",
            Data = historicos
        });
    }

   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var historico =
            await _service.GetByIdAsync(id);

        if (historico == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Histórico não encontrado"
                });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                historico.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Histórico encontrado",
            Data = historico
        });
    }

    

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateHistoricoDto dto
)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                dto.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var historico =
            await _service.CreateAsync(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Histórico criado com sucesso",
            Data = historico
        });
    }

    //

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
      int id,
      [FromBody] UpdateHistoricoDto dto
  )
    {
        var historico =
            await _service.GetByIdAsync(id);

        if (historico == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Histórico não encontrado"
                });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                historico.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var updated =
            await _service.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Histórico não encontrado"
                });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Histórico atualizado com sucesso"
        });
    }

 

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var historico =
            await _service.GetByIdAsync(id);

        if (historico == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Histórico não encontrado"
                });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                historico.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var deleted =
            await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Histórico não encontrado"
                });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Histórico removido com sucesso"
        });
    }
}