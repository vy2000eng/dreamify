using System.Security.Claims;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;

namespace dreamify.Application.Abstracts;

public interface IAccountService
{
    Task<LoginResponse> RegisterUserAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);
    Task<LoginResponse> RefreshTokenAsync(RefreshRequest refreshRequest);
    Task<UserInfoResponse> GetUserInfoAsync(ClaimsPrincipal user);
    Task UpdateUserInfoAsync(InfoRequest infoRequest, ClaimsPrincipal claimsPrincipal);


}