namespace VitaApi.Models;

public class Odontograma
{
    public int Id { get; set; }

    public int Dente { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataRegistro { get; set; }
        = DateTime.UtcNow;

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; }
        = null!;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; }
        = null!;
}