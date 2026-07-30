using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IHistoricoClinicoRepository
{
    Task<List<HistoricoClinico>> GetAllAsync();

    Task<HistoricoClinico?> GetByIdAsync(int id);

    Task AddAsync(HistoricoClinico historico);

    Task UpdateAsync(HistoricoClinico historico);

    Task DeleteAsync(HistoricoClinico historico);

    Task SaveChangesAsync();
}