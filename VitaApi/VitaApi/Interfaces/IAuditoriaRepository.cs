using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IAuditoriaRepository
{
    Task<List<Auditoria>> GetAllAsync();

    Task AddAsync(Auditoria auditoria);

    Task SaveChangesAsync();
}