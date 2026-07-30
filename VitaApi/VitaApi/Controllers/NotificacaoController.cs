using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacaoController : ControllerBase
{
    private readonly INotificacaoService _service;

    public NotificacaoController(
        INotificacaoService service
    )
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var notificacoes =
            await _service.GetUltimasAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Notificações carregadas",
                Data = notificacoes
            }
        );
    }
}