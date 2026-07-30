using FluentValidation;
using VitaApi.DTOs.Auth;

namespace VitaApi.Validators;

public class LoginValidator
    : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email obrigatório")
            .EmailAddress()
            .WithMessage("Email inválido");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha obrigatória")
            .MinimumLength(6)
            .WithMessage(
                "Senha deve ter no mínimo 6 caracteres"
            );
    }
}