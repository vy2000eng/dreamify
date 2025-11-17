using System.Text.Json.Serialization;

namespace dreamify.Domain.Response;


public class UserInfoResponse
{
    [JsonPropertyName("Email")]
    public string Email        {get;set;}
    
    [JsonPropertyName("UserName")]
    public string UserName     {get;set;}
    
    [JsonPropertyName("createdOn ")]
    public DateTime CreatedOn  {get;set;}
    
    [JsonPropertyName("isSubscribed")]
    public bool IsSubscribed   {get;set;}
}