namespace VitaApi.DTOs.Receitas;

public class CreateReceitaDto
{
    public string Medicamento { get; set; }
        = string.Empty;

    public string Dosagem { get; set; }
        = string.Empty;

    public string Frequencia { get; set; }
        = string.Empty;

    public string Duracao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }
}