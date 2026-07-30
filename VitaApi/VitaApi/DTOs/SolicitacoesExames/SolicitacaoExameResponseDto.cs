namespace VitaApi.DTOs.SolicitacoesExames;

public class SolicitacaoExameResponseDto
{
    public int Id { get; set; }

    public string ExamesSolicitados { get; set; }
        = string.Empty;

    public string? Justificativa { get; set; }

    public DateTime DataSolicitacao { get; set; }

    public int ConsultaId { get; set; }

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