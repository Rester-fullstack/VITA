namespace VitaApi.DTOs.Nutricao;

public class CreateNutricaoRegistroDto
{
    public decimal Peso { get; set; }

    public decimal Altura { get; set; }

    public string Objetivo { get; set; }
        = string.Empty;

    public string PlanoAlimentar { get; set; }
        = string.Empty;

    public string Evolucao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }
}