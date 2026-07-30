namespace VitaApi.DTOs.Historicos;

public class HistoricoResponseDto
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int PacienteId { get; set; }

    public int ConsultaId { get; set; }
}