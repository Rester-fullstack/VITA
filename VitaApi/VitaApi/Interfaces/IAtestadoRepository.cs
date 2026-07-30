using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IAtestadoRepository
{
    Task<List<Atestado>> GetAllAsync();

    Task<Atestado?> GetByIdAsync(int id);

    Task AddAsync(Atestado atestado);

    Task SaveChangesAsync();

    Task<bool> DeleteAsync(int id);
}