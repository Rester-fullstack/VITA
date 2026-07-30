using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.DTOs.Medicos;
using VitaApi.Responses;
using VitaApi.Services;
using System.Security.Claims;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicosController : ControllerBase
{
    private readonly MedicoService _service;

    public MedicosController(MedicoService service)
    {
        _service = service;
    }

    private int? GetUserId()
    {
        var claim =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

        return int.TryParse(
            claim,
            out var userId
        )
            ? userId
            : null;
    }

    [HttpGet("meu-perfil")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> GetMeuPerfil()
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Usuário não identificado."
                }
            );
        }

        var perfil =
            await _service.GetMeuPerfilAsync(
                userId.Value
            );

        if (perfil == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Perfil médico não encontrado."
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Perfil carregado com sucesso.",
                Data = perfil
            }
        );
    }

    [HttpPut("meu-perfil")]
    [Authorize(Roles = "Medico")]
    public async Task<IActionResult> UpdateMeuPerfil(
        [FromBody] UpdateMedicoDto dto
    )
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Usuário não identificado."
                }
            );
        }

        var perfil =
            await _service.UpdateMeuPerfilAsync(
                userId.Value,
                dto
            );

        if (perfil == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Perfil médico não encontrado."
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Perfil atualizado com sucesso.",
                Data = perfil
            }
        );
    }

    
    [HttpGet]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetAll()
    {
        var medicos = await _service.GetAllAsync();
        return Ok(
             new ApiResponse<object>
             {
                 Success = true,
                 Message = "Médicos encontrados",
                 Data = medicos
             }
        );
    }

    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetById(int id)
    {
        var medico = await _service.GetByIdAsync(id);

        if (medico == null)
            return NotFound(
                 new ApiResponse<object>
                 {
                     Success = false,
                     Message = "Médico não encontrado"
                 }
            );

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Médico encontrado.",
                Data = medico
            }
        );
    }

    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMedicoDto dto)
    {

        var usuarioId = GetUserId();

        if (usuarioId == null)
        {
            return Unauthorized();
        }

        var usuarioNome =
            User.FindFirst(ClaimTypes.Name)?.Value;

        var usuarioRole =
            User.FindFirst(ClaimTypes.Role)?.Value;


        var medico =
        await _service.CreateAsync(
            dto,
            usuarioId.Value,
            usuarioNome,
            usuarioRole
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = medico.Id },
            new ApiResponse<object>
            {
                Success = true,
                Message = "Médico criado com sucesso",
                Data = medico
            }
        );
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateMedicoDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return Ok(
             new ApiResponse<object>
             {
                 Success = true,
                 Message = "Médico atualizado com sucesso"
             }
        );
    }

   
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Médico removido com sucesso"
            }
        );
    }
}