using FluentValidation;
using VitaApi.DTOs.Pacientes;

namespace VitaApi.Validators;

public class CreatePacienteValidator
    : AbstractValidator<CreatePacienteDto>
{
    public CreatePacienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.CPF)
            .NotEmpty()
            .Length(11);

        RuleFor(x => x.Telefone)
            .NotEmpty();

        RuleFor(x => x.Endereco)
            .NotEmpty();
    }
}