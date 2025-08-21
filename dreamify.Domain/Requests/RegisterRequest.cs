namespace dreamify.Domain.Requests;

public record RegisterRequest
{
    public required String FirstName { get; init; }
    public required String LastName{ get; init; }
    public required String Email { get; init; }
    public required String Password { get; init; }
    
}