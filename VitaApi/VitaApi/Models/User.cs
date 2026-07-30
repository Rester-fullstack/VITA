namespace VitaApi.Models;

public class User
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Medico";

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public ICollection<Consulta> Consultas { get; set; }
        = new List<Consulta>();

    public ICollection<RefreshToken>
    RefreshTokens
    { get; set; }
    = new List<RefreshToken>();
}