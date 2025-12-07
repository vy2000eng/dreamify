using System.Security.Claims;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using dreamify.Domain.Exceptions;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;

namespace dreamify.Application.Services;

public class AccountService:IAccountService
{
    private readonly IGoogleTokenProcessor _googleTokenProcessor;
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager,IUserRepository userRepository, IGoogleTokenProcessor googleTokenProcessor)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
        _googleTokenProcessor = googleTokenProcessor;
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

    public async Task<DeleteUserResponse> DeleteUserInfoAsync(ClaimsPrincipal claimsPrincipal)
    {
          var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? claimsPrincipal.FindFirst("sub")?.Value;
        
            if (string.IsNullOrEmpty(userId))
            {
                throw new UserRetrievalFailure();
            }
        
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                throw new UserRetrievalFailure();            
            }
        
            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded)
            {
                throw new UserDeletionException();
            }
            return new DeleteUserResponse()
            {
                ResultMessage = "Success"
            };
        
    }



    public async Task<LoginResponse> LoginWithGoogle( GoogleLoginRequest request)
    {
        try
        {
            // 1. Verify the Google ID token
            var payload = await _googleTokenProcessor.ValidateGoogleIdToken(request.IdToken);
        
            // 2. Extract user info from the verified token
            var email = payload.Email;
            var googleId = payload.Subject;
            var name = payload.Name;
        
            // 3. Check if user exists in your database
            var user = await _userManager.FindByEmailAsync(email);
        
            if (user == null)
            {
                // 4. Create new user if doesn't exist
                user = User.Create(email);
                user.UserName = name; // Set username from Google
                // You might want to add a GoogleId property to your User entity
            
                var result = await _userManager.CreateAsync(user);
            
                if (!result.Succeeded)
                {
                    throw new RegistrationFailedException(result.Errors.Select(e => e.Description));
                }
            }
        
            // 5. Generate YOUR access and refresh tokens
            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);
        
            // 6. Save refresh token to database
            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiry = refreshTokenExpirationDateInUtc;
            await _userManager.UpdateAsync(user);
        
            var expiresInSeconds = Math.Max(0, (int)(expirationDateInUtc - DateTime.UtcNow).TotalSeconds);
            var expiresIn = expiresInSeconds / 60;
        
            // 7. Return YOUR tokens
            return new LoginResponse
            {
                AccessToken = jwtToken,
                ExpiresIn = expiresIn,
                RefreshToken = refreshTokenValue
            };
        }
        catch (InvalidJwtException)
        {
            throw new UnauthorizedAccessException("Invalid Google token");
        }

    }
}