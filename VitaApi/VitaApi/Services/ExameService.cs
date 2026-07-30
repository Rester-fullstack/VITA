using AutoMapper;

using VitaApi.DTOs.Exames;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class ExameService : IExameService
{
    private readonly IExameRepository
        _repository;

    private readonly IAuditoriaService _auditoriaService;

    private readonly IMapper _mapper;

    public ExameService(
     IExameRepository repository,
     IMapper mapper,
     IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _auditoriaService = auditoriaService;
    }

   

    public async Task<
        List<ExameResponseDto>
    > GetAllAsync()
    {
        var exames =
            await _repository.GetAllAsync();

        return _mapper.Map<
            List<ExameResponseDto>
        >(exames);
    }

  

    public async Task<
        ExameResponseDto?
    > GetByIdAsync(int id)
    {
        var exame =
            await _repository.GetByIdAsync(id);

        if (exame == null)
            return null;

        return _mapper.Map<
            ExameResponseDto
        >(exame);
    }

 
    public async Task<Exame?>
        GetEntityByIdAsync(int id)
    {
        return await
            _repository.GetByIdAsync(id);
    }



    public async Task<ExameResponseDto> CreateAsync(
     CreateExameDto dto,
     int? usuarioId,
     string? usuarioNome,
     string? usuarioRole
    )
    {
        string? pdfUrl = null;

        if (dto.Arquivo != null)
        {
            var uploadsFolder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "exames"
                );

            if (!Directory.Exists(
                uploadsFolder
            ))
            {
                Directory.CreateDirectory(
                    uploadsFolder
                );
            }

            var extension =
                Path.GetExtension(
                    dto.Arquivo.FileName
                );

            var fileName =
                $"{Guid.NewGuid()}{extension}";

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
                await dto.Arquivo.CopyToAsync(
                    stream
                );
            }

            pdfUrl =
                $"/uploads/exames/{fileName}";
        }

        var exame = new Exame
        {
            Nome = dto.Nome,
            Resultado = dto.Resultado,
            DataExame = dto.DataExame,
            ConsultaId = dto.ConsultaId,
            PacienteId = dto.PacienteId,
            PdfUrl = pdfUrl
        };

        await _repository.AddAsync(exame);

        await _repository.SaveChangesAsync();

        var exameCompleto =
        await _repository.GetByIdAsync(exame.Id);

            await _auditoriaService.RegistrarAsync(
                entidade: "Exame",
                acao: "Criou",
                descricao:
                    $"Exame '{exameCompleto!.Nome}' registrado para {exameCompleto.Paciente?.Nome}",
                consultaId: exameCompleto.ConsultaId,
                pacienteId: exameCompleto.PacienteId,
                registroId: exameCompleto.Id,
                icone: "🧬",
                cor: "#8B5CF6"
            );

        return _mapper.Map<ExameResponseDto>(exameCompleto);


    }



    public async Task<bool>
        UpdateAsync(
            int id,
            UpdateExameDto dto
        )
    {
        var exame =
            await _repository.GetByIdAsync(id);

        if (exame == null)
            return false;

        exame.Nome =
            dto.Nome;

        exame.Resultado =
            dto.Resultado;

        exame.DataExame =
            dto.DataExame;

        exame.ConsultaId =
            dto.ConsultaId;

        exame.PacienteId =
            dto.PacienteId;

        await _repository.UpdateAsync(
            exame
        );

        await _repository.SaveChangesAsync();

        return true;
    }



    public async Task<bool>
        DeleteAsync(int id)
    {
        var exame =
            await _repository.GetByIdAsync(id);

        if (exame == null)
            return false;

        await _repository.DeleteAsync(
            exame
        );

        await _repository.SaveChangesAsync();

        return true;
    }



    public async Task
        SaveChangesAsync()
    {
        await _repository
            .SaveChangesAsync();
    }
}