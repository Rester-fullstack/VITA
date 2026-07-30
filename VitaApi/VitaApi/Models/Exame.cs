namespace VitaApi.Models;

public class Exame
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Resultado { get; set; } = string.Empty;

    public string? ArquivoPdf { get; set; }

    public string? PdfUrl { get; set; }

    public DateTime DataExame { get; set; }

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; } = null!;

    public int PacienteId { get; set; }

    public Paciente? Paciente { get; set; }
}