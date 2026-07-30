using AutoMapper;

using VitaApi.DTOs.Historicos;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Services;

public class HistoricoClinicoService
    : IHistoricoClinicoService
{
    private readonly
        IHistoricoClinicoRepository
        _repository;

    private readonly IMapper _mapper;

    public HistoricoClinicoService(
        IHistoricoClinicoRepository repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }




    public async Task<List<HistoricoResponseDto>>
        GetAllAsync()
    {
        var historicos =
            await _repository.GetAllAsync();

        return _mapper.Map<
            List<HistoricoResponseDto>
        >(historicos);
    }


  


    public async Task<HistoricoResponseDto?>
        GetByIdAsync(int id)
    {
        var historico =
            await _repository.GetByIdAsync(id);

        if (historico == null)
            return null;

        return _mapper.Map<
            HistoricoResponseDto
        >(historico);
    }


   

    public async Task<HistoricoResponseDto>
        CreateAsync(CreateHistoricoDto dto)
    {
        var historico =
            _mapper.Map<HistoricoClinico>(
                dto
            );

        historico.DataRegistro =
            DateTime.UtcNow;

        await _repository.AddAsync(
            historico
        );

        await _repository.SaveChangesAsync();

        return _mapper.Map<
            HistoricoResponseDto
        >(historico);
    }





    public async Task<bool>
        UpdateAsync(
            int id,
            UpdateHistoricoDto dto
        )
    {
        var historico =
            await _repository.GetByIdAsync(id);

        if (historico == null)
            return false;

        historico.Descricao =
            dto.Descricao;

        await _repository.UpdateAsync(
            historico
        );

        await _repository.SaveChangesAsync();

        return true;
    }




    public async Task<bool>
        DeleteAsync(int id)
    {
        var historico =
            await _repository.GetByIdAsync(id);

        if (historico == null)
            return false;

        await _repository.DeleteAsync(
            historico
        );

        await _repository.SaveChangesAsync();

        return true;
    }
}