using FluentValidation;
using VitaApi.DTOs.Exames;

namespace VitaApi.Validators;

public class UpdateExameValidator
    : AbstractValidator<UpdateExameDto>
{
    public UpdateExameValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Resultado)
            .NotEmpty();

        RuleFor(x => x.DataExame)
            .NotEmpty();
    }
}