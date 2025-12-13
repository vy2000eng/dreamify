namespace dreamify.Domain.Requests;

public record VerificationRequest
{ 
    public required string VerificationRequestCode{ get; init; }
    public required string Email{get;init;}
    //public required string LoginType{ get; init; }
    
    
}