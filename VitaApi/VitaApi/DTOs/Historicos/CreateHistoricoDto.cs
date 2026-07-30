namespace VitaApi.DTOs.Historicos;

public class CreateHistoricoDto
{
    public string Descricao { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public int ConsultaId { get; set; }
}