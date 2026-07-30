namespace VitaApi.DTOs.Prontuarios;

public class ProntuarioPacienteDto
{
    public int PacienteId { get; set; }

    public string PacienteNome { get; set; } = string.Empty;

    public string? CPF { get; set; }

    public string? Telefone { get; set; }

    public DateTime? DataNascimento { get; set; }

    public List<ProntuarioConsultaDto> Consultas { get; set; } = new();
}