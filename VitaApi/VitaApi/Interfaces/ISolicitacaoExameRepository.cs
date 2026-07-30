using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface ISolicitacaoExameRepository
{
    Task<List<SolicitacaoExame>> GetAllAsync();

    Task<List<SolicitacaoExame>> GetByConsultaIdAsync(
    int consultaId
);

    Task<SolicitacaoExame?> GetByIdAsync(int id);

    Task AddAsync(SolicitacaoExame solicitacao);

    Task DeleteAsync(SolicitacaoExame solicitacao);

    Task SaveChangesAsync();
}