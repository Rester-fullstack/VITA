using System.Xml.Linq;

namespace VitaApi.Models;

public class Consulta
{
    public int Id { get; set; }

    public DateTime DataConsulta { get; set; }

    public string Status { get; set; } = "Agendada";

    public string Observacoes { get; set; } = string.Empty;

    public int PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    public int MedicoId { get; set; }

    public Medico Medico { get; set; } = null!;

    public ICollection<Exame> Exames { get; set; }
        = new List<Exame>();

    public ICollection<SolicitacaoExame> SolicitacoesExames
    { get; set; } = new List<SolicitacaoExame>();

    public ICollection<Receita> Receitas { get; set; } =
    new List<Receita>();

    public ICollection<Atestado> Atestados { get; set; } =
        new List<Atestado>();

    public ICollection<DeclaracaoComparecimento> DeclaracoesComparecimento { get; set; } =
        new List<DeclaracaoComparecimento>();

    public ICollection<HistoricoClinico> HistoricosClinicos { get; set; } =
    new List<HistoricoClinico>();
}