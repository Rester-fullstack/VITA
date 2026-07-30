using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IConsultaRepository
{
    Task<List<Consulta>> GetAllAsync();

    Task<List<Consulta>> GetByUserIdAsync(int userId);

    Task<int?> GetMedicoIdByUserIdAsync(int userId);

    Task<Consulta?> GetByIdAsync(int id);

    Task AddAsync(Consulta consulta);

    Task UpdateAsync(Consulta consulta);

    Task DeleteAsync(Consulta consulta);

    Task SaveChangesAsync();

    Task<bool> ExistsConflictAsync(int medicoId, DateTime data);
    Task<Consulta?> GetByIdForDeleteAsync(int id);
}