using VitaApi.Models;

namespace VitaApi.Interfaces
{
    public interface IAgendaRepository
    {
        Task<List<AgendaMedica>> GetByMedicoAsync(int medicoId);

        Task<List<Consulta>> GetByUserIdAsync(int userId);

        Task AddAsync(AgendaMedica agenda);

        Task SaveChangesAsync();
    }
}
