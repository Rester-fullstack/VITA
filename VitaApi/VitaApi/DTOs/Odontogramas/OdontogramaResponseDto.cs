namespace VitaApi.DTOs.Odontogramas;

public class OdontogramaResponseDto
{
    public int Id { get; set; }

    public int Dente { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }

    public string PacienteNome { get; set; }
        = string.Empty;
}