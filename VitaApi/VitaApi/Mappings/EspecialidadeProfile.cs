
using AutoMapper;

using VitaApi.DTOs.Especialidades;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class EspecialidadeProfile : Profile
{
    public EspecialidadeProfile()
    {
        CreateMap<Especialidade,
            EspecialidadeResponseDto>();

        CreateMap<CreateEspecialidadeDto,
            Especialidade>();
    }
}