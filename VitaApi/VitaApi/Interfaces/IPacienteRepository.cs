using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IPacienteRepository
{
    Task<List<Paciente>> GetAllAsync();

    Task<List<Paciente>>
    GetPagedAsync(
        int page,
        int pageSize
    );

    Task<Paciente?> GetByIdAsync(int id);

    Task AddAsync(Paciente paciente);

    Task UpdateAsync(Paciente paciente);

    Task DeleteAsync(Paciente paciente);

    Task SaveChangesAsync();
}