namespace VitaApi.DTOs.Notificacoes;

public class NotificacaoDto
{
    public int Id { get; set; }

    public string Titulo { get; set; } = "";

    public string Descricao { get; set; } = "";

    public string Icone { get; set; } = "";

    public string Cor { get; set; } = "";

    public DateTime DataHora { get; set; }
}