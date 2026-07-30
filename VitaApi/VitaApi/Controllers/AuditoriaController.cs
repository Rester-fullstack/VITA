using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _service;

    public AuditoriaController(
        IAuditoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista =
            await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Auditoria encontrada.",
                Data = lista
            });
    }
}