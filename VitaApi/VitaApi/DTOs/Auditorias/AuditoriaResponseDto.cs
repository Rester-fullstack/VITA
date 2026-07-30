namespace VitaApi.DTOs.Auditorias;

public class AuditoriaResponseDto
{
    public int Id { get; set; }

    public string Entidade { get; set; } = string.Empty;

    public string Acao { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataHora { get; set; }

    public int? UsuarioId { get; set; }

    public string? UsuarioNome { get; set; }

    public string? UsuarioRole { get; set; }

    public int? ConsultaId { get; set; }

    public int? PacienteId { get; set; }

    public int? RegistroId { get; set; }
}