using System.Text.Json.Serialization;

namespace dreamify.Domain.Response;

public class LoginResponse
{
    
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; } // seconds until expiration
    
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}