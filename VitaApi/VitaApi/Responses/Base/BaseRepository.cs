using VitaApi.Data;
using VitaApi.Interfaces;

namespace VitaApi.Repositories.Base;

public class BaseRepository<T>
    : IBaseRepository<T>
    where T : class
{
    protected readonly AppDbContext _context;

    public BaseRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>()
            .AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _context.Set<T>()
            .Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _context.Set<T>()
            .Remove(entity);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}