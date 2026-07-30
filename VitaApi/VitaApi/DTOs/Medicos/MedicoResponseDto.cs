namespace VitaApi.DTOs.Medicos;

public class MedicoResponseDto
{
    public int Id { get; set; }

    public string CRM { get; set; } =
        string.Empty;

    public string Nome { get; set; } =
        string.Empty;

    public string Email { get; set; } =
        string.Empty;

    public int EspecialidadeId { get; set; }

    public string Especialidade { get; set; } =
        string.Empty;

    public string? Telefone { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? EnderecoProfissional { get; set; }

    public string? Assinatura { get; set; }
}