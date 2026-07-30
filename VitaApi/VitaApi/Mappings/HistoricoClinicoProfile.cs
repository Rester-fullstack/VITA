
using AutoMapper;

using VitaApi.DTOs.Historicos;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class HistoricoClinicoProfile : Profile
{
    public HistoricoClinicoProfile()
    {
        CreateMap<HistoricoClinico,
            HistoricoResponseDto>();

        CreateMap<CreateHistoricoDto,
            HistoricoClinico>();
    }
}