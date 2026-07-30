using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimelineController : ControllerBase
{
    private readonly ITimelineService _timelineService;
    private readonly IConsultaService _consultaService;

    public TimelineController(
        ITimelineService timelineService,
        IConsultaService consultaService
    )
    {
        _timelineService = timelineService;
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
    public async Task<IActionResult> GetTimelinePaciente(
        int pacienteId
    )
    {
        var podeAcessar =
            await MedicoPodeAcessarPaciente(
                pacienteId
            );

        if (!podeAcessar)
            return Forbid();

        var timeline =
            await _timelineService
                .GetTimelinePacienteAsync(
                    pacienteId
                );

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Timeline carregada com sucesso",
                Data = timeline
            }
        );
    }
}