namespace VitaApi.Models;

public class Atestado
{
    public int Id { get; set; }

    public string Motivo { get; set; }
        = string.Empty;

    public string Cid { get; set; }
        = string.Empty;

    public DateTime DataInicio { get; set; }

    public int DiasAfastamento { get; set; }

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataEmissao { get; set; }
        = DateTime.UtcNow;

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; }
        = null!;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; }
        = null!;
}