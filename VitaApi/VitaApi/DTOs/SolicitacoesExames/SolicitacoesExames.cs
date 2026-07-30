namespace VitaApi.DTOs.SolicitacoesExames;

public class CreateSolicitacaoExameDto
{
    public string ExamesSolicitados { get; set; } = string.Empty;

    public string? Justificativa { get; set; }

    public int ConsultaId { get; set; }
}