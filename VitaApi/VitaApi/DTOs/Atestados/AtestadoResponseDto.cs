namespace VitaApi.DTOs.Atestados;

public class AtestadoResponseDto
{
    public int Id { get; set; }

    public string Motivo { get; set; }
        = string.Empty;

    public string Cid { get; set; }
        = string.Empty;

    public DateTime DataInicio { get; set; }

    public int DiasAfastamento { get; set; }

    public string Observacoes { get; set; }
        = string.Empty;

    public DateTime DataEmissao { get; set; }

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