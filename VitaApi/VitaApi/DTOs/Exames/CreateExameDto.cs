namespace VitaApi.DTOs.Exames;

public class CreateExameDto
{
    public string Nome { get; set; } = string.Empty;

    public string Resultado { get; set; } = string.Empty;

    public DateTime DataExame { get; set; }

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }

    public IFormFile? Arquivo { get; set; }
}