using dreamify.Application.Abstracts;
using Google.Apis.Auth;
using System.Threading.Tasks;
using dreamify.Domain.Exceptions;
using dreamify.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace dreamify.Infrastructure.Processors;

public class GoogleTokenProcessor:IGoogleTokenProcessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;
    
    public GoogleTokenProcessor(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
    }
    
    
    
    
    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleIdToken(string idToken)
    {
        try
        {
      
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _jwtOptions.GoogleAudience } // Your Google Client ID
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (InvalidJwtException ex)
        {
            throw new CustomInvalidJwtException(ex.Message);

            //throw  new InvalidJwtException(ex.Message);
            // Handle invalid token (e.g., expired, invalid signature, incorrect audience)
            // Log the exception or return null/throw a specific exception
        }
    }
}