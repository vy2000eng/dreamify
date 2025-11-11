using Microsoft.AspNetCore.Identity;

namespace dreamify.Domain.Entities;

public class User:IdentityUser<Guid>
{
    // public required string FirstName { get; set; }
    // public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    
    public DateTime CreatedOn { get; set; }

    public Boolean IsSubscribed { get; set; }
    public Boolean DoesHaveRecordings { get; set; }
    public Boolean AreRecordsDownloadedToClients { get; set; }
    
    

    public static User Create(string email,string? refreshToken = null)
    {
        return new User
        {
            Email                         = email,
            UserName                      = email,
            CreatedOn                     = DateTime.UtcNow,
            IsSubscribed                  = false,
            DoesHaveRecordings            = false,
            AreRecordsDownloadedToClients = false,
            
        };
    }

    public override string ToString()
    {
        return Email + " " + CreatedOn.ToShortDateString();
        //return FirstName + " " + LastName;
    }

  
    
}