namespace VitaApi.DTOs.Medicos;

public class UpdateMedicoDto
{
    public string Nome { get; set; } =
        string.Empty;

    public string Email { get; set; } =
        string.Empty;

    public string CRM { get; set; } =
        string.Empty;

    public int EspecialidadeId { get; set; }

    public string? Telefone { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? EnderecoProfissional { get; set; }

    public string? Assinatura { get; set; }
}