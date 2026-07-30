namespace VitaApi.Models;

public class Medico
{
    public int Id { get; set; }

    public string CRM { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? EnderecoProfissional { get; set; }

    public string? Assinatura { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int EspecialidadeId { get; set; }

    public Especialidade Especialidade { get; set; } = null!;

    public ICollection<Consulta> Consultas { get; set; }
        = new List<Consulta>();
}