using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IOdontogramaRepository
{
    Task<List<Odontograma>> GetAllAsync();

    Task<Odontograma?> GetByIdAsync(int id);

    Task AddAsync(Odontograma odontograma);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync();
}