using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IReceitaRepository
{
    Task<List<Receita>> GetAllAsync();

    Task<Receita?> GetByIdAsync(int id);

    Task AddAsync(Receita receita);

    Task SaveChangesAsync();

    Task<bool> DeleteAsync(int id);
}