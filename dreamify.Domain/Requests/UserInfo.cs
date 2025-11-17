namespace dreamify.Domain.Requests;

public record UserInfo
{
    public required String Email { get; init; }
    public required String Password { get; init; }
    
}