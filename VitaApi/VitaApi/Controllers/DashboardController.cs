using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using VitaApi.Responses;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(
        DashboardService service
    )
    {
        _service = service;
    }

   

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> Admin()
    {
        var data =
            await _service.GetAdminDashboardAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Dashboard admin carregado",
            Data = data
        });
    }

    

    [Authorize(Roles = "Medico")]
    [HttpGet("medico")]
    public async Task<IActionResult> Medico()
    {
        var medicoIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

        if (medicoIdClaim == null)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não autenticado"
                });
        }

        var userIdClaim =
             User.FindFirst(
                 ClaimTypes.NameIdentifier
             )?.Value;

        if (userIdClaim == null)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não autenticado"
                });
        }

        var userId =
            int.Parse(userIdClaim);

        var data =
            await _service
                .GetMedicoDashboardAsync(
                    userId
                );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Dashboard médico carregado",
            Data = data
        });
    }
}