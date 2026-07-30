namespace VitaApi.Models;

public class DeclaracaoComparecimento
{
    public int Id { get; set; }

    public string? Observacoes { get; set; }

    public DateTime DataEmissao { get; set; }

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; } = null!;
}