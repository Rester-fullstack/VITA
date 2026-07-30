namespace VitaApi.DTOs.Consultas;

public class ConsultaResponseDto
{
    public int Id { get; set; }

    public DateTime DataConsulta { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public string PacienteNome { get; set; } = string.Empty;

    public int MedicoId { get; set; }

    public int MedicoUserId { get; set; }

    public string MedicoNome { get; set; } = string.Empty;

    public bool PodeExcluir { get; set; }
}