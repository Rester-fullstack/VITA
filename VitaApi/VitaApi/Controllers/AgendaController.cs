using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgendaController : ControllerBase
{
    private readonly IAgendaRepository _repository;

    public AgendaController(
        IAgendaRepository repository
    )
    {
        _repository = repository;
    }

    [HttpGet("minha")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> GetMinhaAgenda()
    {
        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            );

        if (userIdClaim == null)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não identificado"
                }
            );
        }

        var userId =
            int.Parse(userIdClaim.Value);

        var agenda =
            await _repository.GetByUserIdAsync(userId);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Agenda carregada com sucesso",
                Data = agenda
            }
        );
    }
}