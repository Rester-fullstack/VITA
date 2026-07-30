namespace VitaApi.DTOs.ConfiguracoesClinica;

public class ConfiguracaoClinicaDto
{
    public int Id { get; set; }

    public string NomePlataforma { get; set; }
        = string.Empty;

    public string EmailSuporte { get; set; }
        = string.Empty;

    public string TelefoneSuporte { get; set; }
        = string.Empty;

    public string WhatsappSuporte { get; set; }
        = string.Empty;

    public string RodapePdf { get; set; }
        = string.Empty;

    public string MensagemPadrao { get; set; }
        = string.Empty;

    public DateTime AtualizadoEm { get; set; }
}