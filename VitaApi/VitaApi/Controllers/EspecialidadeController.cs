using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Especialidades;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EspecialidadeController : ControllerBase
{
    private readonly IEspecialidadeService _service;

    public EspecialidadeController(
        IEspecialidadeService service
    )
    {
        _service = service;
    }

    

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var especialidades =
            await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Especialidades encontradas",
            Data = especialidades
        });
    }

  

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateEspecialidadeDto dto
    )
    {
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


        var especialidade =
    await _service.CreateAsync(
        dto,
        usuarioId,
        usuarioNome,
        usuarioRole
    );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Especialidade criada com sucesso",
            Data = especialidade
        });
    }
}