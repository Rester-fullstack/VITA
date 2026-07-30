using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IEspecialidadeRepository
{
    Task<List<Especialidade>> GetAllAsync();

    Task<Especialidade?> GetByIdAsync(int id);

    Task AddAsync(Especialidade especialidade);

    Task UpdateAsync(
        Especialidade especialidade
    );

    Task DeleteAsync(
        Especialidade especialidade
    );

    Task SaveChangesAsync();
}