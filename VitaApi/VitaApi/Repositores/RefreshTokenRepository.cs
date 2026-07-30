using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;

namespace VitaApi.Repositories;

public class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task AddAsync(
        RefreshToken refreshToken
    )
    {
        await _context.RefreshTokens
            .AddAsync(refreshToken);
    }

    public async Task<RefreshToken?>
        GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Token == token
            );
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}