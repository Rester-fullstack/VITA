using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Nutricao;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NutricaoController : ControllerBase
{
    private readonly INutricaoService _service;
    private readonly IConsultaService _consultaService;

    public NutricaoController(
        INutricaoService service,
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
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetAll()
    {
        var registros =
            await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Registros de nutrição encontrados",
                Data = registros
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id
    )
    {
        var registro =
            await _service.GetByIdAsync(id);

        if (registro == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Registro não encontrado"
                }
            );
        }

        var podeAcessar =
        await MedicoPodeAcessarConsulta(
            registro.ConsultaId
        );

        if (!podeAcessar)
            return Forbid();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Registro encontrado",
                Data = registro
            }
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    CreateNutricaoRegistroDto dto
)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                dto.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var registro =
            await _service.CreateAsync(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Registro de nutrição criado com sucesso",
                Data = registro
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id
    )
    {
        var registro =
    await _service.GetByIdAsync(id);

        if (registro == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Registro não encontrado"
                }
            );
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                registro.ConsultaId
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
                    Message = "Registro não encontrado"
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Registro removido com sucesso"
            }
        );
    }
}