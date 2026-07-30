using FluentValidation;
using VitaApi.DTOs.Consultas;

namespace VitaApi.Validators;

public class CreateConsultaValidator
    : AbstractValidator<CreateConsultaDto>
{
    public CreateConsultaValidator()
    {
        RuleFor(x => x.DataConsulta)
            .NotEmpty();

        RuleFor(x => x.Observacoes)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.PacienteId)
            .GreaterThan(0);

        RuleFor(x => x.MedicoId)
            .GreaterThan(0);
    }
}