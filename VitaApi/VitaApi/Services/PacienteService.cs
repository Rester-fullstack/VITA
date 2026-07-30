using VitaApi.DTOs.Pacientes;
using VitaApi.Interfaces;
using VitaApi.Models;
using AutoMapper;

namespace VitaApi.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _repository;

    private readonly IAuditoriaService _auditoriaService;

    private readonly IMapper _mapper;

    public PacienteService(
         IPacienteRepository repository,
         IMapper mapper,
         IAuditoriaService auditoriaService
     )
    {
        _repository = repository;
        _mapper = mapper;
        _auditoriaService = auditoriaService;
    }




    public async Task<List<PacienteResponseDto>>
        GetAllAsync()
    {
        var pacientes =
            await _repository.GetAllAsync();

        return _mapper.Map<
            List<PacienteResponseDto>
        >(pacientes);
    }




    public async Task<List<PacienteResponseDto>>
        GetPagedAsync(
            int page,
            int pageSize
        )
    {
        var pacientes =
            await _repository.GetPagedAsync(
                page,
                pageSize
            );

        return pacientes
            .Select(p => new PacienteResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                Telefone = p.Telefone,
                DataNascimento =
                    p.DataNascimento,
                Endereco = p.Endereco,
                CreatedAt = p.CreatedAt
            })
            .ToList();
    }

    


    public async Task<PacienteResponseDto?>
        GetByIdAsync(int id)
    {
        var paciente =
            await _repository.GetByIdAsync(id);

        if (paciente == null)
            return null;

        return _mapper.Map<
            PacienteResponseDto
        >(paciente);
    }




    public async Task<PacienteResponseDto> CreateAsync(
     CreatePacienteDto dto,
     int? usuarioId,
     string? usuarioNome,
     string? usuarioRole
 )
    {
        var paciente =
            _mapper.Map<Paciente>(dto);

        await _repository.AddAsync(paciente);

        await _repository.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(
            entidade: "Paciente",
            acao: "Criou",
            descricao: $"Paciente {paciente.Nome} cadastrado",
            pacienteId: paciente.Id,
            registroId: paciente.Id,
            icone: "👤",
            cor: "#14B8A6"
        );

        return _mapper.Map<PacienteResponseDto>(paciente);
    }





    public async Task<bool> UpdateAsync(int id, UpdatePacienteDto dto)
    {
        var paciente =
            await _repository.GetByIdAsync(id);

        if (paciente == null)
            return false;

        paciente.Nome = dto.Nome;
        paciente.Telefone = dto.Telefone;
        paciente.DataNascimento = dto.DataNascimento;
        paciente.Endereco = dto.Endereco;

        await _repository.UpdateAsync(paciente);

        await _repository.SaveChangesAsync();

        return true;
    }


 


    public async Task<bool> DeleteAsync(int id)
    {
        var paciente =
           await _repository.GetByIdAsync(id);

        if (paciente == null)
            return false;

        await _repository.DeleteAsync(paciente);

        await _repository.SaveChangesAsync();

        return true;
    }
}