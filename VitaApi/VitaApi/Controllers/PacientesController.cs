using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaApi.DTOs.Pacientes;
using VitaApi.Interfaces;
using VitaApi.Responses;
using System.Security.Claims;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/pacientes")]
[Authorize]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    private readonly IConsultaService _consultaService;

    public PacientesController(
        IPacienteService pacienteService,
        IConsultaService consultaService
    )
    {
        _pacienteService = pacienteService;
        _consultaService = consultaService;
    }


    private async Task<bool> UsuarioPodeAcessarPaciente(int pacienteId)
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



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10
    )
    {
        pageSize = Math.Min(pageSize, 50);

        var pacientes =
            await _pacienteService
                .GetPagedAsync(
                    page,
                    pageSize
                );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Pacientes encontrados",
            Data = pacientes
        });
    }

  

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente =
            await _pacienteService
                .GetByIdAsync(id);

        if (paciente == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Paciente não encontrado"
                });
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

            var minhasConsultas =
                await _consultaService
                    .GetMyConsultasAsync(userId);

            var temPermissao =
                minhasConsultas.Any(c =>
                    c.PacienteId == id
                );

            if (!temPermissao)
            {
                return Forbid();
            }
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Paciente encontrado",
            Data = paciente
        });
    }

   

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePacienteDto dto
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


        var paciente =
     await _pacienteService
         .CreateAsync(
             dto,
             usuarioId,
             usuarioNome,
             usuarioRole
         );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Paciente criado com sucesso",
            Data = paciente
        });
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePacienteDto dto
    )
    {
        var podeAcessar =
            await UsuarioPodeAcessarPaciente(id);

        if (!podeAcessar)
            return Forbid();


        var updated =
            await _pacienteService
                .UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Paciente não encontrado"
                });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Paciente atualizado com sucesso"
        });
    }

   

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var podeAcessar =
        await UsuarioPodeAcessarPaciente(id);

        if (!podeAcessar)
            return Forbid();


        var deleted =
            await _pacienteService
                .DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Paciente não encontrado"
                });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Paciente removido com sucesso"
        });
    }
}