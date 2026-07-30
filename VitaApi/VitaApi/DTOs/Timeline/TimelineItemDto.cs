namespace VitaApi.DTOs.Timeline;

public class TimelineItemDto
{
    public DateTime Data { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int ConsultaId { get; set; }

    public int? DocumentoId { get; set; }
}