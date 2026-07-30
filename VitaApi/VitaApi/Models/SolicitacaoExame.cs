namespace VitaApi.Models;

public class SolicitacaoExame
{
    public int Id { get; set; }

    public string ExamesSolicitados { get; set; } = string.Empty;

    public string? Justificativa { get; set; }

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;

    public int ConsultaId { get; set; }
    public Consulta Consulta { get; set; } = null!;
}