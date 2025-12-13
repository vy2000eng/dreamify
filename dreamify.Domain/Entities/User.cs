using Microsoft.AspNetCore.Identity;

namespace dreamify.Domain.Entities;

public class User:IdentityUser<Guid>
{
 
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    
    public DateTime CreatedOn { get; set; }

    public Boolean IsSubscribed { get; set; }
    
    public string? GoogleId { get; set; } // Store the subject here
    public string AuthProvider { get; set; } = "Local";
    public string? EmailVerificationCode { get; set; }
    public DateTime? EmailVerificationCodeExpiry { get; set; }
// public Boolean DoesHaveRecordings { get; set; }
    // public Boolean AreRecordsDownloadedToClients { get; set; }
    
    

    public static User Create(string email, string? refreshToken = null, string authProvider = "Local")
    {
        return new User
        {
            Email        = email,
            UserName     = email,
            CreatedOn    = DateTime.UtcNow,
            IsSubscribed = false,
            AuthProvider = authProvider  // Add this
        };
    }

    public override string ToString()
    {
        return Email + " " + CreatedOn.ToShortDateString();
    }

  
    
}