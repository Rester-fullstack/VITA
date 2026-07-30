using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Consultas;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaService _service;

    public ConsultaController(
        IConsultaService service
    )
    {
        _service = service;
    }

    private async Task<bool> UsuarioPodeAcessarConsulta(int consultaId)
    {
        var consulta =
            await _service.GetByIdAsync(consultaId);

        if (consulta == null)
            return false;

        var role =
            User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
            return true;

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (role != "Medico" || userIdClaim == null)
            return false;

        var userId =
            int.Parse(userIdClaim.Value);

        return consulta.MedicoUserId == userId;
    }

    

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var consultas =
            await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Consultas encontradas",
                Data = consultas
            }
        );
    }

    [HttpGet("minhas")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> GetMyConsultas()
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

        var consultas =
            await _service.GetMyConsultasAsync(
                userId
            );

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Consultas do médico",
                Data = consultas
            }
        );
    }

   

    [HttpGet("{id}")]
    public async Task<IActionResult>
     GetById(int id)
    {
        var consulta =
            await _service.GetByIdAsync(id);

        if (consulta == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Consulta não encontrada"
                }
            );
        }

        var role =
            User.FindFirst(ClaimTypes.Role)?.Value;

        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            );

        if (
            role == "Medico" &&
            userIdClaim != null
        )
        {
            var userId =
                int.Parse(userIdClaim.Value);

            if (consulta.MedicoUserId != userId)
            {
                return Forbid();
            }
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Consulta encontrada",
                Data = consulta
            }
        );
    }

   

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult>
     Create(CreateConsultaDto dto)
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

        var consulta =
            await _service.CreateAsync(
                dto,
                usuarioId,
                usuarioNome,
                usuarioRole
            );

        return CreatedAtAction(
            nameof(GetById),

            new { id = consulta.Id },

            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Consulta criada com sucesso",
                Data = consulta
            }
        );
    }

    [HttpPost("minha")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult>
    CreateMinhaConsulta(
        CreateMinhaConsultaDto dto
    )
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

        var usuarioNome =
            User.FindFirst(
                ClaimTypes.Name
            )?.Value;

                var usuarioRole =
                    User.FindFirst(
                        ClaimTypes.Role
                    )?.Value;

        var consulta =
           await _service.CreateMinhaConsultaAsync(
                userId,
                usuarioNome,
                usuarioRole,
                dto
            );
        return CreatedAtAction(
            nameof(GetById),
            new { id = consulta.Id },
            new ApiResponse<object>
            {
                Success = true,
                Message = "Consulta criada com sucesso",
                Data = consulta
            }
        );
    }

   

    [HttpPut("{id}")]
    public async Task<IActionResult>
        Update(
            int id,
            UpdateConsultaDto dto
        )
    {
        var podeAcessar =
            await UsuarioPodeAcessarConsulta(id);

        if (!podeAcessar)
            return Forbid();

        var updated =
            await _service.UpdateAsync(
                id,
                dto
            );

        if (!updated)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Consulta não encontrada"
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Consulta atualizada com sucesso"
            }
        );
    }

    

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        Delete(int id)
    {

        var podeAcessar =
        await UsuarioPodeAcessarConsulta(id);

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
                    Message =
                        "Consulta não encontrada"
                }
            );
        }

        return NoContent();
    }
}