namespace VitaApi.DTOs.Consultas;

public class CreateMinhaConsultaDto
{
    public int PacienteId { get; set; }

    public DateTime DataConsulta { get; set; }

    public string? Observacoes { get; set; }
}