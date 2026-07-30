namespace VitaApi.DTOs.Psicologia;

public class CreatePsicologiaRegistroDto
{
    public string Humor { get; set; } = string.Empty;

    public string QueixaPrincipal { get; set; }
        = string.Empty;

    public string EvolucaoSessao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }
}