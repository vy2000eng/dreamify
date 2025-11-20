namespace dreamify.Domain.Requests;

public record InfoRequest
{
    public required String NewUserName { get; init; }
    public required String NewPassword { get; init; }
    public required String OldPassword { get; init; }

    
    
    
    

}