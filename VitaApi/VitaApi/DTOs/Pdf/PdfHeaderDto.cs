namespace VitaApi.DTOs.Pdf;

public class PdfHeaderDto
{
    public string Titulo { get; set; } = "";

    public int DocumentoId { get; set; }

    public string PacienteNome { get; set; } = "";


    public string MedicoNome { get; set; } = "";
    public string MedicoCrm { get; set; } = "";
    public string Especialidade { get; set; } = "";
    public string TelefoneMedico { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Estado { get; set; } = "";
    public string EnderecoProfissional { get; set; } = "";
    public string Assinatura { get; set; } = "";

    
    public string NomeClinica { get; set; } = "VITA";
    public string Subtitulo { get; set; } = "Sistema Inteligente de Gestão Clínica";
    public string TelefoneClinica { get; set; } = "";
    public string EmailClinica { get; set; } = "";
    public string EnderecoClinica { get; set; } = "";
    public string Rodape { get; set; } = "";
}