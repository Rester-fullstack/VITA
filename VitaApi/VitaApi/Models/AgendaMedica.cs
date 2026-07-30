namespace VitaApi.Models;

public class AgendaMedica
{
    public int Id { get; set; }

    public int MedicoId { get; set; }

    public Medico Medico { get; set; } = null!;

    public DateTime DataHora { get; set; }

    public bool Ocupado { get; set; }

    public string Tipo { get; set; } = "Livre"; 

    public int? ConsultaId { get; set; }

    public Consulta? Consulta { get; set; }
}