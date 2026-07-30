using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    

    [Authorize]
    [HttpGet("perfil")]
    public IActionResult Perfil()
    {
        var nome =
            User.Identity?.Name;

        var role =
            User.Claims
                .FirstOrDefault(
                    x => x.Type.Contains("role")
                )
                ?.Value;

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuário autenticado",
            Data = new
            {
                nome,
                role
            }
        });
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Área Admin liberada"
        });
    }



    [Authorize(Roles = "Admin,Medico")]
    [HttpGet("medico")]
    public IActionResult Medico()
    {
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Área Médico liberada"
        });
    }
}