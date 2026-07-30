namespace VitaApi.DTOs.Medicos;

public class CreateMedicoDto
{
    public string CRM { get; set; } =
        string.Empty;

    public int UserId { get; set; }

    public int EspecialidadeId { get; set; }

    public string? Telefone { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? EnderecoProfissional { get; set; }

    public string? Assinatura { get; set; }
}