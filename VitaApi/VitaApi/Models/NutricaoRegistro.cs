namespace VitaApi.Models;

public class NutricaoRegistro
{
    public int Id { get; set; }

    public decimal Peso { get; set; }

    public decimal Altura { get; set; }

    public decimal Imc { get; set; }

    public string Objetivo { get; set; }
        = string.Empty;

    public string PlanoAlimentar { get; set; }
        = string.Empty;

    public string Evolucao { get; set; }
        = string.Empty;

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataRegistro { get; set; }
        = DateTime.UtcNow;

    public int ConsultaId { get; set; }

    public Consulta Consulta { get; set; }
        = null!;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; }
        = null!;
}