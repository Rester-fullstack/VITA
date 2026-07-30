using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IDeclaracaoComparecimentoRepository
{
    Task<List<DeclaracaoComparecimento>> GetAllAsync();

    Task<List<DeclaracaoComparecimento>> GetByConsultaIdAsync(
        int consultaId
    );

    Task<DeclaracaoComparecimento?> GetByIdAsync(
        int id
    );

    Task AddAsync(
        DeclaracaoComparecimento declaracao
    );

    Task DeleteAsync(
        DeclaracaoComparecimento declaracao
    );

    Task SaveChangesAsync();
}