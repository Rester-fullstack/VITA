using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface INutricaoRepository
{
    Task<List<NutricaoRegistro>> GetAllAsync();

    Task<NutricaoRegistro?> GetByIdAsync(int id);

    Task AddAsync(NutricaoRegistro registro);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync();
}