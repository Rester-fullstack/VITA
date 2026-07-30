namespace VitaApi.Interfaces;

public interface IBaseRepository<T>
{
    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task SaveChangesAsync();
}