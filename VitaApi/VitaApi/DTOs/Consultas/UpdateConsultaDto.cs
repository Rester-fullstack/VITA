namespace VitaApi.DTOs.Consultas;

public class UpdateConsultaDto
{
    public DateTime DataConsulta { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }
}