using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dreamify.Infrastructure.Repository;

public class UserRepository: IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByRefreshToken(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        return user;
    }
}