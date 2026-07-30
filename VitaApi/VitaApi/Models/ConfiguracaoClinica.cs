namespace VitaApi.Models;

public class ConfiguracaoClinica
{
    public int Id { get; set; }

    public string NomePlataforma { get; set; } = "VITA";

    public string EmailSuporte { get; set; } = string.Empty;

    public string TelefoneSuporte { get; set; } = string.Empty;

    public string WhatsappSuporte { get; set; } = string.Empty;

    public string RodapePdf { get; set; } =
        "Documento emitido eletronicamente pela plataforma VITA.";

    public string MensagemPadrao { get; set; } = string.Empty;

    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}