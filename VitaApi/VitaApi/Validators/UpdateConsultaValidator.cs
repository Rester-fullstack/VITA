using FluentValidation;
using VitaApi.DTOs.Consultas;

namespace VitaApi.Validators;

public class UpdateConsultaValidator
    : AbstractValidator<UpdateConsultaDto>
{
    public UpdateConsultaValidator()
    {
        RuleFor(x => x.DataConsulta)
            .NotEmpty();

        RuleFor(x => x.Observacoes)
            .NotEmpty()
            .MinimumLength(3);
    }
}