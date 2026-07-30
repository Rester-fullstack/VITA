using AutoMapper;

using VitaApi.DTOs.Especialidades;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class EspecialidadeService
    : IEspecialidadeService
{
    private readonly IEspecialidadeRepository
        _repository;

    private readonly IAuditoriaService _auditoriaService;

    private readonly IMapper _mapper;

    public EspecialidadeService(
        IEspecialidadeRepository repository,
        IMapper mapper,
        IAuditoriaService auditoriaService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _auditoriaService = auditoriaService;
    }

    public async Task<
        List<EspecialidadeResponseDto>
    > GetAllAsync()
    {
        var especialidades =
            await _repository.GetAllAsync();

        return _mapper.Map<
            List<EspecialidadeResponseDto>
        >(especialidades);
    }

    public async Task<EspecialidadeResponseDto> CreateAsync(
    CreateEspecialidadeDto dto,
    int? usuarioId,
    string? usuarioNome,
    string? usuarioRole
)
    {
        var especialidade =
            _mapper.Map<Especialidade>(dto);

        await _repository.AddAsync(especialidade);

        await _repository.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(
            entidade: "Especialidade",
            acao: "Criou",
            descricao: $"Especialidade {especialidade.Nome} cadastrada",
            registroId: especialidade.Id,
            icone: "🏥",
            cor: "#6366F1"
        );

        return _mapper.Map<EspecialidadeResponseDto>(
            especialidade
        );
    }
}