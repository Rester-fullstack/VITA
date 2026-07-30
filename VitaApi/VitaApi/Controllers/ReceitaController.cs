using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Receitas;
using VitaApi.Interfaces;
using VitaApi.Responses;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReceitaController : ControllerBase
{
    private readonly IReceitaService _service;
    private readonly ReceitaPdfService _pdfService;
    private readonly IConsultaService _consultaService;
    private readonly IUserContextService _userContext;

    public ReceitaController(
        IReceitaService service,
        ReceitaPdfService pdfService,
        IConsultaService consultaService,
        IUserContextService userContext
    )
    {
        _service = service;
        _pdfService = pdfService;
        _consultaService = consultaService;
        _userContext = userContext;
    }

    private async Task<bool> MedicoPodeAcessarConsulta(int consultaId)
    {
        if (_userContext.IsAdmin)
            return true;

        if (!_userContext.IsMedico || _userContext.UserId == null)
            return false;

        var minhasConsultas =
            await _consultaService.GetMyConsultasAsync(
                _userContext.UserId.Value
            );

        return minhasConsultas.Any(c => c.Id == consultaId);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetAll()
    {
        var receitas = await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Receitas encontradas",
            Data = receitas
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var receita = await _service.GetByIdAsync(id);

        if (receita == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Receita não encontrada"
            });
        }

        if (!await MedicoPodeAcessarConsulta(receita.ConsultaId))
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Receita encontrada",
            Data = receita
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReceitaDto dto
    )
    {
        if (!await MedicoPodeAcessarConsulta(dto.ConsultaId))
            return Forbid();

        var receita =
            await _service.CreateAsync(
                dto,
                _userContext.UserId,
                _userContext.UserName,
                _userContext.Role
            );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Receita criada com sucesso",
            Data = receita
        });
    }

    [HttpGet("pdf/{id}")]
    public async Task<IActionResult> GerarPdf(int id)
    {
        var receita = await _service.GetByIdAsync(id);

        if (receita == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Receita não encontrada"
            });
        }

        if (!await MedicoPodeAcessarConsulta(receita.ConsultaId))
            return Forbid();

        var pdf = await _pdfService.GerarPdfAsync(receita);

        return File(
            pdf,
            "application/pdf",
            $"receita-{id}.pdf"
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var receita = await _service.GetByIdAsync(id);

        if (receita == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Receita não encontrada"
            });
        }

        if (!await MedicoPodeAcessarConsulta(receita.ConsultaId))
            return Forbid();

        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Receita não encontrada"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Receita removida com sucesso"
        });
    }
}