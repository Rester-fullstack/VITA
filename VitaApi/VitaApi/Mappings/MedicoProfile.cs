using AutoMapper;
using VitaApi.DTOs.Medico;
using VitaApi.DTOs.Medicos;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class MedicoProfile : Profile
{
    public MedicoProfile()
    {
        // Model -> DTO
        CreateMap<Medico, MedicoDto>()
            .ForMember(dest => dest.Nome,
                opt => opt.MapFrom(src => src.User.Nome))
            .ForMember(dest => dest.Especialidade,
                opt => opt.MapFrom(src => src.Especialidade.Nome));

        // DTO -> Model
        CreateMap<CreateMedicoDto, Medico>();

        CreateMap<UpdateMedicoDto, Medico>();
    }
}