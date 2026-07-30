namespace VitaApi.DTOs.Exames;

public class ExameResponseDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Resultado { get; set; } = string.Empty;

    public DateTime DataExame { get; set; }

    public string? PdfUrl { get; set; }

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }

    public string PacienteNome { get; set; } = string.Empty;
}