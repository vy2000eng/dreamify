namespace dreamify.Domain.Requests;

public record RefreshRequest
{
    public required String RefreshToken { get; set; }

}