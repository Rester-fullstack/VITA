using FluentValidation;
using VitaApi.DTOs.Exames;

namespace VitaApi.Validators;

public class CreateExameValidator
    : AbstractValidator<CreateExameDto>
{
    public CreateExameValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Resultado)
            .NotEmpty();

        RuleFor(x => x.DataExame)
            .NotEmpty();

        RuleFor(x => x.ConsultaId)
            .GreaterThan(0);

        RuleFor(x => x.PacienteId)
            .GreaterThan(0);
    }
}