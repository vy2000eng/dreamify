namespace dreamify.Domain.Requests;

public record GoogleLoginRequest
{


    public required string IdToken { get; set; }
}