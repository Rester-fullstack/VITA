using AutoMapper;

using VitaApi.DTOs.Pacientes;
using VitaApi.Models;

namespace VitaApi.Mappings;

public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
        CreateMap<Paciente,
            PacienteResponseDto>();

        CreateMap<CreatePacienteDto,
            Paciente>();
    }
}