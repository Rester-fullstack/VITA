namespace VitaApi.DTOs.ConfiguracoesClinica;

public class UpdateConfiguracaoClinicaDto
{
    public string NomePlataforma { get; set; } = string.Empty;

    public string EmailSuporte { get; set; } = string.Empty;

    public string TelefoneSuporte { get; set; } = string.Empty;

    public string WhatsappSuporte { get; set; } = string.Empty;

    public string RodapePdf { get; set; } = string.Empty;

    public string MensagemPadrao { get; set; } = string.Empty;
}