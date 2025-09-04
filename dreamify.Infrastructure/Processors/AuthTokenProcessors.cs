using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using dreamify.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace dreamify.Infrastructure.Processors;

public class AuthTokenProcessor:IAuthTokenProcessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;
    
    public AuthTokenProcessor(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
    }

    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credential = new SigningCredentials  (signingKey, SecurityAlgorithms.HmacSha256);
        var claims     = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Only use ID here
            new Claim(ClaimTypes.Name, user.Email), // Use email as name
            new Claim(ClaimTypes.Email, user.Email)
        };

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);
        var token           = new JwtSecurityToken
        (
            issuer            :_jwtOptions.Issuer,
            audience          :_jwtOptions.Audience,
            claims            :claims,
            expires           :expires,
            signingCredentials:credential
        );
         
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return (jwtToken,expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
        
    }

    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, 
            new CookieOptions
            {
                HttpOnly = true,
                Expires = expiration,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            }
        );
      
        
    }
    
    //public void WriteAuthTokenAsAuthorizationToken
    
}