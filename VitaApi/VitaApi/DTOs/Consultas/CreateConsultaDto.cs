namespace VitaApi.DTOs.Consultas;

public class CreateConsultaDto
{
    public DateTime DataConsulta { get; set; }

    public string Observacoes { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }
}