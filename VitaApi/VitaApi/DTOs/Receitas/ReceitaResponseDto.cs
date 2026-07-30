namespace VitaApi.DTOs.Receitas;

public class ReceitaResponseDto
{
    public int Id { get; set; }

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

    public DateTime DataReceita { get; set; }

    public int ConsultaId { get; set; }

    public int PacienteId { get; set; }

    public string PacienteNome { get; set; }
        = string.Empty;

    
    public string MedicoNome { get; set; }
        = string.Empty;

    public string MedicoCrm { get; set; }
        = string.Empty;

    public string MedicoEspecialidade { get; set; }
        = string.Empty;

    public string MedicoTelefone { get; set; }
        = string.Empty;

    public string MedicoCidade { get; set; }
        = string.Empty;

    public string MedicoEstado { get; set; }
        = string.Empty;

    public string MedicoEnderecoProfissional { get; set; }
        = string.Empty;

    public string MedicoAssinatura { get; set; }
        = string.Empty;
}