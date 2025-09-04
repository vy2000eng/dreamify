using System.Text.Json.Serialization;

namespace dreamify.Domain.Response;

public class AnalysisResponse
{
    [JsonPropertyName("dreamAnalysisResponse")]
    public string? DreamAnalysisResponse { get; set; }
}