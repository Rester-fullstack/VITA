using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.SolicitacoesExames;
using VitaApi.Interfaces;
using VitaApi.Responses;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolicitacaoExameController : ControllerBase
{
    private readonly ISolicitacaoExameService _service;
    private readonly SolicitacaoExamePdfService _pdfService;
    private readonly IConsultaService _consultaService;

    public SolicitacaoExameController(
        ISolicitacaoExameService service,
        SolicitacaoExamePdfService pdfService,
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
        var lista = await _service.GetAllAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Solicitações encontradas",
            Data = lista
        });
    }

    [HttpGet("consulta/{consultaId}")]
    public async Task<IActionResult> GetByConsulta(int consultaId)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(consultaId);

        if (!podeAcessar)
            return Forbid();

        var lista =
            await _service.GetByConsultaIdAsync(consultaId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Solicitações encontradas",
            Data = lista
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var solicitacao =
            await _service.GetByIdAsync(id);

        if (solicitacao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Solicitação não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                solicitacao.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Solicitação encontrada",
            Data = solicitacao
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSolicitacaoExameDto dto
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

        var solicitacao =
            await _service.CreateAsync(
                dto,
                usuarioId,
                usuarioNome,
                usuarioRole
            );

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Solicitação criada com sucesso",
            Data = solicitacao
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var solicitacao =
            await _service.GetByIdAsync(id);

        if (solicitacao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Solicitação não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                solicitacao.ConsultaId
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
                Message = "Solicitação não encontrada"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Solicitação removida com sucesso"
        });
    }

    [HttpGet("pdf/{id}")]
    public async Task<IActionResult> GerarPdf(int id)
    {
        var solicitacao =
            await _service.GetByIdAsync(id);

        if (solicitacao == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Solicitação não encontrada"
            });
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                solicitacao.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var pdf =
          await _pdfService.GerarPdfAsync(solicitacao);

        return File(
            pdf,
            "application/pdf",
            $"solicitacao-exame-{id}.pdf"
        );
    }
}