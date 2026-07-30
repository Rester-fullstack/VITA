namespace VitaApi.DTOs.Atestados;

public class CreateAtestadoDto
{
    public string Motivo { get; set; }
        = string.Empty;

    public string Cid { get; set; }
        = string.Empty;

    public DateTime DataInicio { get; set; }

    public int DiasAfastamento { get; set; }

    public string Observacoes { get; set; }
        = string.Empty;

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }
}