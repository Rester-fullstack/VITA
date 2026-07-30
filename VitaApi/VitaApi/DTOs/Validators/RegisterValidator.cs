using FluentValidation;
using VitaApi.DTOs.Auth;

namespace VitaApi.Validators;

public class RegisterValidator
    : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome obrigatório")
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role =>
                role == "Admin"
                || role == "Medico"
            )
            .WithMessage(
                "Role deve ser Admin ou Medico"
            );
    }
}