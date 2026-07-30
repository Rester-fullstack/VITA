using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IPsicologiaRepository
{
    Task<List<PsicologiaRegistro>> GetAllAsync();

    Task<PsicologiaRegistro?> GetByIdAsync(int id);

    Task AddAsync(PsicologiaRegistro registro);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync();
}