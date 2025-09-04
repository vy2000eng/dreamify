namespace dreamify.Domain.Requests;

public record AnalysisRequest
{
    public required String TextToAnalyze { get; set; }
}