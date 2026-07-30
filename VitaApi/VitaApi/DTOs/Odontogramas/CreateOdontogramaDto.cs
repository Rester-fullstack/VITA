namespace VitaApi.DTOs.Odontogramas;

public class CreateOdontogramaDto
{
    public int Dente { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }
}