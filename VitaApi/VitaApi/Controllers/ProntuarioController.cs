using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Responses;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProntuarioController : ControllerBase
{
    private readonly IProntuarioService _prontuarioService;
    private readonly ProntuarioPdfService _pdfService;
    private readonly IConsultaService _consultaService;

    public ProntuarioController(
        IProntuarioService prontuarioService,
        ProntuarioPdfService pdfService,
        IConsultaService consultaService
    )
    {
        _prontuarioService = prontuarioService;
        _pdfService = pdfService;
        _consultaService = consultaService;
    }

    private async Task<bool> MedicoPodeAcessarPaciente(
        int pacienteId
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
            c.PacienteId == pacienteId
        );
    }

    [HttpGet("paciente/{pacienteId}")]
    public async Task<IActionResult> GetProntuario(
        int pacienteId
    )
    {
        var podeAcessar =
            await MedicoPodeAcessarPaciente(
                pacienteId
            );

        if (!podeAcessar)
            return Forbid();

        var prontuario =
            await _prontuarioService
                .GetPacienteAsync(
                    pacienteId
                );

        if (prontuario == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Paciente não encontrado"
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Prontuário encontrado",
                Data = prontuario
            }
        );
    }

    [HttpGet("paciente/{pacienteId}/pdf")]
    public async Task<IActionResult> GerarPdf(
        int pacienteId
    )
    {
        var podeAcessar =
            await MedicoPodeAcessarPaciente(
                pacienteId
            );

        if (!podeAcessar)
            return Forbid();

        var prontuario =
            await _prontuarioService
                .GetPacienteAsync(
                    pacienteId
                );

        if (prontuario == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Paciente não encontrado"
                }
            );
        }

        var pdf = await _pdfService.GerarPdfAsync(prontuario);

        return File(
             pdf,
             "application/pdf",
             $"prontuario-{prontuario.PacienteNome.Replace(" ", "-")}.pdf"
         );
    }
}