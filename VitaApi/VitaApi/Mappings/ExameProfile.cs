using AutoMapper;

using VitaApi.DTOs.Exames;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class ExameProfile : Profile
{
    public ExameProfile()
    {
        CreateMap<Exame, ExameResponseDto>()
            .ForMember(
                dest => dest.PacienteNome,
                opt => opt.MapFrom(
                    src => src.Paciente!.Nome
                )
            );

        CreateMap<CreateExameDto, Exame>();

        CreateMap<UpdateExameDto, Exame>();
    }
}