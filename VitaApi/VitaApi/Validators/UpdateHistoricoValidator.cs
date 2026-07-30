using FluentValidation;
using VitaApi.DTOs.Historicos;

namespace VitaApi.Validators;

public class UpdateHistoricoValidator
    : AbstractValidator<UpdateHistoricoDto>
{
    public UpdateHistoricoValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty()
            .MinimumLength(3);
    }
}