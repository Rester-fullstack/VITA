using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitaApi.DTOs.Exames;
using VitaApi.Interfaces;
using VitaApi.Responses;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExameController : ControllerBase
{
    private readonly IExameService _service;
    private readonly IConsultaService _consultaService;

    public ExameController(
        IExameService service,
        IConsultaService consultaService
    )
    {
        _service = service;
        _consultaService = consultaService;
    }

    private async Task<bool> MedicoPodeAcessarConsulta(
    int consultaId
)
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
            c.Id == consultaId
        );
    }

    

    [HttpGet]
    [Authorize(Roles = "Admin,Medico")]
    public async Task<IActionResult> GetAll()
    {
        var exames =
            await _service.GetAllAsync();


        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Exames encontrados",
            Data = exames
        });
    }

    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id
    )
    {
        var exame =
            await _service.GetByIdAsync(id);

        if (exame == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message =
                        "Exame não encontrado"
                }
            );
        }


        var podeAcessar =
        await MedicoPodeAcessarConsulta(
            exame.ConsultaId
        );

        if (!podeAcessar)
            return Forbid();


        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Exame encontrado",
                Data = exame
            }
        );
    }

    

    [HttpPost]
    public async Task<IActionResult> Create(
    [FromForm] CreateExameDto dto
)
    {
        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                dto.ConsultaId
            );

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

                var exame =
             await _service.CreateAsync(
                 dto,
                 usuarioId,
                 usuarioNome,
                 usuarioRole
             );

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "Exame criado com sucesso",
                Data = exame
            }
        );
    }

    

    [HttpPut("{id}")]
    public async Task<IActionResult>
        Update(
            int id,
            [FromBody]
            UpdateExameDto dto
        )

    {

        var exame =
        await _service.GetByIdAsync(id);

        if (exame == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Exame não encontrado"
                }
            );
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                exame.ConsultaId
            );

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
                        "Exame não encontrado"
                }
            );
        }



        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Exame atualizado com sucesso"
            }
        );
    }

   

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        Delete(int id)
    {

        var exame =
            await _service.GetByIdAsync(id);

        if (exame == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Exame não encontrado"
                }
            );
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                exame.ConsultaId
            );

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
                        "Exame não encontrado"
                }
            );
        }

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message =
                    "Exame removido com sucesso"
            }
        );
    }


    [HttpPost("upload/{id}")]
    public async Task<IActionResult> UploadPdf(
     int id,
     [FromForm] IFormFile file
 )
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Arquivo inválido"
                }
            );
        }

        var exame =
            await _service.GetEntityByIdAsync(id);

        if (exame == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Exame não encontrado"
                }
            );
        }

        var podeAcessar =
            await MedicoPodeAcessarConsulta(
                exame.ConsultaId
            );

        if (!podeAcessar)
            return Forbid();

        var uploadsFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "exames"
            );

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName =
            $"{Guid.NewGuid()}_{file.FileName}";

        var filePath =
            Path.Combine(
                uploadsFolder,
                fileName
            );

        using (
            var stream =
                new FileStream(
                    filePath,
                    FileMode.Create
                )
        )
        {
            await file.CopyToAsync(stream);
        }

        exame.PdfUrl =
            $"/uploads/exames/{fileName}";

        await _service.SaveChangesAsync();

        return Ok(
            new ApiResponse<object>
            {
                Success = true,
                Message = "PDF enviado com sucesso",
                Data = exame.PdfUrl
            }
        );
    }
}