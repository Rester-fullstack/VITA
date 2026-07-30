namespace VitaApi.DTOs.Dashboard;

public class DashboardTimelineDto
{
    public string Entidade { get; set; } = "";

    public string Acao { get; set; } = "";

    public string Descricao { get; set; } = "";

    public string? Usuario { get; set; }

    public DateTime DataHora { get; set; }

    public string? Icone { get; set; }

    public string? Cor { get; set; }
}