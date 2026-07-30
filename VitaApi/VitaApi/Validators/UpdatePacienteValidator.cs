using FluentValidation;
using VitaApi.DTOs.Pacientes;

namespace VitaApi.Validators;

public class UpdatePacienteValidator
    : AbstractValidator<UpdatePacienteDto>
{
    public UpdatePacienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Telefone)
            .NotEmpty();

        RuleFor(x => x.Endereco)
            .NotEmpty();
    }
}