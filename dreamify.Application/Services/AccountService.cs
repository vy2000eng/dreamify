using System.Security.Claims;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using dreamify.Domain.Exceptions;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;
using Microsoft.AspNetCore.Identity;

namespace dreamify.Application.Services;

public class AccountService:IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager,IUserRepository userRepository)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
    }


    public async Task<LoginResponse> RegisterUserAsync(RegisterRequest registerRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

        if (userExists)
        {
            throw new UserAlreadyExistsException(email: registerRequest.Email);
        }
        
        var user = User.Create(registerRequest.Email);

        var result = await _userManager.CreateAsync(user, registerRequest.Password);


        if (!result.Succeeded)
        {
            throw new RegistrationFailedException(result.Errors.Select(e => e.Description));
        }
           
        // Generate tokens for immediate login
        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
    
        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiry = refreshTokenExpirationDateInUtc;
        var expiresInSeconds = Math.Max(0, (int)(expirationDateInUtc - DateTime.UtcNow).TotalSeconds);
        var expiresIn = expiresInSeconds / 60;

        return new LoginResponse
        {
            AccessToken = jwtToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshTokenValue
        };

    }


    public async Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            throw new LoginFailedException(loginRequest.Email);

        }

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiry = refreshTokenExpirationDateInUtc;
        
        await _userManager.UpdateAsync(user);
        var expiresInSeconds = Math.Max(0, (int)(expirationDateInUtc - DateTime.UtcNow).TotalSeconds);
        var expiresIn = expiresInSeconds / 60;

        
        return new LoginResponse
        {
            AccessToken = jwtToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshTokenValue
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshRequest refreshRequest)
    {
        if (string.IsNullOrEmpty(refreshRequest.RefreshToken))
        {
            throw new RefreshTokenException("Refresh token is missing.");
        }
        var user = await _userRepository.GetUserByRefreshToken(refreshRequest.RefreshToken);
        if (user == null)
        {
            throw new RefreshTokenException("Refresh token is invalid.");
        }

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new RefreshTokenException("Refresh token has expired.");
        }
        
        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiry = refreshTokenExpirationDateInUtc;
        
        await _userManager.UpdateAsync(user);
        var expiresInSeconds = Math.Max(0, (int)(expirationDateInUtc - DateTime.UtcNow).TotalSeconds);
        var expiresIn = expiresInSeconds / 60;

        
        return new LoginResponse
        {
            AccessToken = jwtToken,
            ExpiresIn = expiresIn,
            RefreshToken = refreshTokenValue
        };
        
        
        
    }



    public async Task<UserInfoResponse> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
    
        if (string.IsNullOrEmpty(userId))
        {
            //return null;
            throw new UserRetrievalFailure();
        }
    
        // Fetch the full user from your database
        var user = await _userManager.FindByIdAsync(userId);
    
        if (user == null)
        {
            //TODO:add an actual err
            return null;
            
        }
    
        // Create response object
        return new UserInfoResponse
        {
            Email = user.Email,
            UserName = user.UserName,
            CreatedOn = user.CreatedOn,
            IsSubscribed = user.IsSubscribed,
        };

    }


    public async Task UpdateUserInfoAsync(InfoRequest infoRequest, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new UserRetrievalFailure();
        }
    
        // Fetch the full user from your database
        var user = await _userManager.FindByIdAsync(userId);
        

        //update username 
        if (!string.IsNullOrWhiteSpace(infoRequest.NewUserName) && user.UserName != infoRequest.NewUserName)
        {
            var usernameResult = await _userManager.SetUserNameAsync(user, infoRequest.NewUserName);
            
            if (!usernameResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update username: {string.Join(", ", usernameResult.Errors.Select(e => e.Description))}");
            }
        }
        
        // Update password if provided
        if (!string.IsNullOrWhiteSpace(infoRequest.NewPassword))
        {
            // You'd need to add OldPassword to InfoRequest for this approach
            var passwordResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
    
            if (!passwordResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update password: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
            }
        }
        
    }
    
    
    
    
    
}