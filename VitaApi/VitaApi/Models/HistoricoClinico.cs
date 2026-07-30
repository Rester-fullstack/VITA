namespace VitaApi.Models;

public class HistoricoClinico
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }
        = DateTime.UtcNow;

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; } = null!;

    public int PacienteId { get; set; }

    public Paciente? Paciente { get; set; }
}