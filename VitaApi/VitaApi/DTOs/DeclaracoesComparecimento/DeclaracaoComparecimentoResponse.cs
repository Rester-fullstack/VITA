namespace VitaApi.DTOs.DeclaracoesComparecimento;

public class DeclaracaoComparecimentoResponseDto
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }

    public string PacienteNome { get; set; } = string.Empty;

   
    public string MedicoNome { get; set; } = string.Empty;

    public string MedicoCrm { get; set; } = string.Empty;

    public string MedicoEspecialidade { get; set; } = string.Empty;

    public string MedicoTelefone { get; set; } = string.Empty;

    public string MedicoCidade { get; set; } = string.Empty;

    public string MedicoEstado { get; set; } = string.Empty;

    public string MedicoEnderecoProfissional { get; set; } = string.Empty;

    public string MedicoAssinatura { get; set; } = string.Empty;

    public DateTime DataConsulta { get; set; }

    public DateTime DataEmissao { get; set; }

    public string? Observacoes { get; set; }
}