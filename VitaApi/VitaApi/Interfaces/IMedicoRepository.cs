using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IMedicoRepository
{
    Task<List<Medico>> GetAllAsync();

    Task<Medico?> GetByIdAsync(int id);

    Task AddAsync(Medico medico);

    Task UpdateAsync(Medico medico);

    Task DeleteAsync(Medico medico);

    Task SaveChangesAsync();

    Task<Medico?> GetByUserIdAsync(int userId);

}