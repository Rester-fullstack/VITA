using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.DeclaracoesComparecimento;
using VitaApi.Interfaces;
using VitaApi.Responses;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeclaracaoComparecimentoController : ControllerBase
{
    private readonly IDeclaracaoComparecimentoService _service;
    private readonly DeclaracaoComparecimentoPdfService _pdfService;
    private readonly IConsultaService _consultaService;

    public DeclaracaoComparecimentoController(
        IDeclaracaoComparecimentoService service,
        DeclaracaoComparecimentoPdfService pdfService,
        IConsultaService consultaService
    )
    {
        _service = service;
        _pdfService = pdfService;
        _consultaService = consultaService;
    }

    private async Task<bool> MedicoPodeAcessarConsulta(int consultaId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
            return true;

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (role != "Medico" || userIdClaim == null)
            return false;

        var userId = int.Parse(userIdClaim.Value);

        var minhasConsultas =
            await _consultaService.GetMyConsultasAsync(userId);

        return minhasConsultas.Any(c => c.Id == consultaId);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var declaracoes = await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Declarações encontradas",
            Data = declaracoes
        });
    }

    [HttpGet("consulta/{consultaId}")]
    public async Task<IActionResult> GetByConsulta(int consultaId)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(consultaId);

        if (!podeAcessar)
            return Forbid();

        var declaracoes =
            await _service.GetByConsultaIdAsync(consultaId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Declarações encontradas",
            Data = declaracoes
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var declaracao =
            await _service.GetByIdAsync(id);

        if (declaracao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Declaração não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                declaracao.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Declaração encontrada",
            Data = declaracao
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDeclaracaoComparecimentoDto dto
    )
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(dto.ConsultaId);

        if (!podeAcessar)
            return Forbid();

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

        var declaracao =
            await _service.CreateAsync(
                dto,
                usuarioId,
                usuarioNome,
                usuarioRole
            );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Declaração criada com sucesso",
            Data = declaracao
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var declaracao =
            await _service.GetByIdAsync(id);

        if (declaracao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Declaração não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                declaracao.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var deleted =
            await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Declaração não encontrada"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Declaração removida com sucesso"
        });
    }

    [HttpGet("pdf/{id}")]
    public async Task<IActionResult> GerarPdf(int id)
    {
        var declaracao =
            await _service.GetByIdAsync(id);

        if (declaracao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Declaração não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                declaracao.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var pdf =
          await _pdfService.GerarPdfAsync(declaracao);

        return File(
            pdf,
            "application/pdf",
            $"declaracao-comparecimento-{id}.pdf"
        );
    }
}