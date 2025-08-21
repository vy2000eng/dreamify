using dreamify.Domain.Entities;

namespace dreamify.Application.Abstracts;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshToken(string refreshToken);
}