using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IExameRepository
{
    Task<List<Exame>> GetAllAsync();

    Task<Exame?> GetByIdAsync(int id);

    Task AddAsync(Exame exame);

    Task UpdateAsync(Exame exame);

    Task DeleteAsync(Exame exame);

    Task SaveChangesAsync();
}