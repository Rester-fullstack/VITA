namespace VitaApi.Models;

public class PsicologiaRegistro
{
    public int Id { get; set; }

    public string Humor { get; set; } = string.Empty;

    public string QueixaPrincipal { get; set; } = string.Empty;

    public string EvolucaoSessao { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }
        = DateTime.Now;

    public int ConsultaId { get; set; }
    public Consulta Consulta { get; set; } = null!;

    public int PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;
}