using System.Text.Json.Serialization;

namespace dreamify.Domain.Response;

public class DeleteUserResponse
{
    [JsonPropertyName("result")]

    public string ResultMessage { get; set; }
}