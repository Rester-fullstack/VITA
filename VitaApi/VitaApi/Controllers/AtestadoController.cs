using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.DTOs.Atestados;
using VitaApi.Interfaces;
using VitaApi.Responses;
using VitaApi.Services;
using System.Security.Claims;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AtestadoController : ControllerBase
{
    private readonly IAtestadoService _service;
    private readonly AtestadoPdfService _pdfService;
    private readonly IConsultaService _consultaService;

    public AtestadoController(
        IAtestadoService service,
        AtestadoPdfService pdfService,
        IConsultaService consultaService
    )
    {
        _service = service;
        _pdfService = pdfService;
        _consultaService = consultaService;
    }

    private async Task<bool> MedicoPodeAcessarConsulta(int consultaId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
            return true;

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (role != "Medico" || userIdClaim == null)
            return false;

        var userId = int.Parse(userIdClaim.Value);

        var minhasConsultas =
            await _consultaService.GetMyConsultasAsync(userId);

        return minhasConsultas.Any(c => c.Id == consultaId);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetAll()
    {
        var atestados = await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Atestados encontrados",
            Data = atestados
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var atestado = await _service.GetByIdAsync(id);

        if (atestado == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Atestado não encontrado"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(atestado.ConsultaId);

        if (!podeAcessar)
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Atestado encontrado",
            Data = atestado
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAtestadoDto dto
    )
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(dto.ConsultaId);

        if (!podeAcessar)
            return Forbid();

        var usuarioId =
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

        var usuarioNome =
            User.FindFirst(ClaimTypes.Name)?.Value;

        var usuarioRole =
            User.FindFirst(ClaimTypes.Role)?.Value;

        var atestado =
            await _service.CreateAsync(
                dto,
                usuarioId,
                usuarioNome,
                usuarioRole
            );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Atestado criado com sucesso",
            Data = atestado
        });
    }

    [HttpGet("pdf/{id}")]
    public async Task<IActionResult> GerarPdf(int id)
    {
        var atestado = await _service.GetByIdAsync(id);

        if (atestado == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Atestado não encontrado"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(atestado.ConsultaId);

        if (!podeAcessar)
            return Forbid();

        var pdf =
        await _pdfService.GerarPdfAsync(atestado);

        return File(
            pdf,
            "application/pdf",
            $"atestado-{id}.pdf"
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var atestado = await _service.GetByIdAsync(id);

        if (atestado == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Atestado não encontrado"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(atestado.ConsultaId);

        if (!podeAcessar)
            return Forbid();

        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Atestado não encontrado"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Atestado removido com sucesso"
        });
    }
}