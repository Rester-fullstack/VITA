namespace VitaApi.DTOs.Psicologia;

public class PsicologiaRegistroResponseDto
{
    public int Id { get; set; }

    public string Humor { get; set; } = string.Empty;

    public string QueixaPrincipal { get; set; }
        = string.Empty;

    public string EvolucaoSessao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }

    public string PacienteNome { get; set; }
        = string.Empty;
}