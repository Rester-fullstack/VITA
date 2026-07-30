using VitaApi.Models;

namespace VitaApi.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(
        RefreshToken refreshToken
    );

    Task<RefreshToken?>
        GetByTokenAsync(string token);

    Task SaveChangesAsync();
}