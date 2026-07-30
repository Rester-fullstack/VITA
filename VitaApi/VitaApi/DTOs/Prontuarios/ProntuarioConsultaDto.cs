namespace VitaApi.DTOs.Prontuarios;

public class ProntuarioConsultaDto
{
    public int ConsultaId { get; set; }

    public DateTime DataConsulta { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Observacoes { get; set; }

    public string MedicoNome { get; set; } = string.Empty;

    public string MedicoCrm { get; set; } = string.Empty;

    public string MedicoEspecialidade { get; set; } = string.Empty;

    public string MedicoTelefone { get; set; } = string.Empty;

    public string MedicoCidade { get; set; } = string.Empty;

    public string MedicoEstado { get; set; } = string.Empty;

    public string MedicoEnderecoProfissional { get; set; } = string.Empty;

    public string MedicoAssinatura { get; set; } = string.Empty;

    public List<string> Historicos { get; set; } = new();

    public List<string> Receitas { get; set; } = new();

    public List<string> Atestados { get; set; } = new();

    public List<string> Declaracoes { get; set; } = new();

    public List<string> SolicitacoesExames { get; set; } = new();

    public List<string> Exames { get; set; } = new();
}