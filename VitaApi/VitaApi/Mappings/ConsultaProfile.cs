
using AutoMapper;

using VitaApi.DTOs.Consultas;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class ConsultaProfile : Profile
{
    public ConsultaProfile()
    {
        CreateMap<Consulta,
            ConsultaResponseDto>()
            .ForMember(
                dest => dest.PacienteNome,
                opt => opt.MapFrom(
                    src => src.Paciente.Nome
                )
            )
            .ForMember(
                dest => dest.MedicoNome,
                opt => opt.MapFrom(
                    src => src.Medico.User.Nome
                )
            )
            .ForMember(
                dest => dest.MedicoUserId,
                opt => opt.MapFrom(src => src.Medico.UserId)
            );

        CreateMap<CreateConsultaDto,
            Consulta>();
    }
}