namespace VitaApi.Models;

public class Especialidade
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public ICollection<Medico> Medicos { get; set; }
        = new List<Medico>();
}