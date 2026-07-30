namespace VitaApi.Models;

public class Receita
{
    public int Id { get; set; }

    public string Medicamento { get; set; }
        = string.Empty;

    public string Dosagem { get; set; }
        = string.Empty;

    public string Frequencia { get; set; }
        = string.Empty;

    public string Duracao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataReceita { get; set; }
        = DateTime.UtcNow;

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; }
        = null!;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; }
        = null!;
}