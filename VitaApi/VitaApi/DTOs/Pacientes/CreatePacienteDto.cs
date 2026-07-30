namespace VitaApi.DTOs.Pacientes;

public class CreatePacienteDto
{
    public string Nome { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string Endereco { get; set; } = string.Empty;
}