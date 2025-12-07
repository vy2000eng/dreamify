using Google.Apis.Auth;

namespace dreamify.Application.Abstracts;

public interface IGoogleTokenProcessor
{
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleIdToken(string idToken);

}