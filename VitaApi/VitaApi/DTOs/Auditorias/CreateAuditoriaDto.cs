namespace VitaApi.DTOs.Auditorias;

public class CreateAuditoriaDto
{
    public string Entidade { get; set; } = string.Empty;

    public string Acao { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int? ConsultaId { get; set; }

    public int? PacienteId { get; set; }

    public int? RegistroId { get; set; }
}