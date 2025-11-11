namespace dreamify.Domain.Requests;

public record RegisterRequest
{
    public required String Email { get; init; }
    public required String Password { get; init; }
    
}