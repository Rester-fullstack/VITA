using Microsoft.EntityFrameworkCore;

using VitaApi.Data;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Repositories.Base;

namespace VitaApi.Repositories;

public class UserRepository
    : BaseRepository<User>,
      IUserRepository
{
    public UserRepository(
        AppDbContext context
    ) : base(context)
    {
    }

    public async Task<User?>
        GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email
            );
    }

    public async Task<User?>
        GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id
            );
    }
}