using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.DTOs.Odontogramas;
using VitaApi.Interfaces;
using VitaApi.Responses;
using System.Security.Claims;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OdontogramaController : ControllerBase
{
    private readonly IOdontogramaService _service;
    private readonly IConsultaService _consultaService;

    public OdontogramaController(
        IOdontogramaService service,
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
        var odontogramas =
            await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Odontogramas encontrados",
                Data = odontogramas
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id
    )
    {
        var odontograma =
            await _service.GetByIdAsync(id);

        if (odontograma == null)
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
            odontograma.ConsultaId
        );

        if (!podeAcessar)
            return Forbid();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Registro encontrado",
                Data = odontograma
            }
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(
    CreateOdontogramaDto dto
)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                dto.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var odontograma =
            await _service.CreateAsync(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Odontograma registrado com sucesso",
                Data = odontograma
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id
    )
    {

        var odontograma =
        await _service.GetByIdAsync(id);

        if (odontograma == null)
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
                odontograma.ConsultaId
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