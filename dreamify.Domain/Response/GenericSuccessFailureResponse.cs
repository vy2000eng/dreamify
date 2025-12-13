using System.Text.Json.Serialization;

namespace dreamify.Domain.Response;

public class GenericSuccessFailureResponse
{
    [JsonPropertyName("result")]
    public string ResultMessage{get; set;}
    
    
}