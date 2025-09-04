namespace dreamify.Application.Abstracts;

public interface IOpenAiRequestProcessor
{
    StringContent GenerateDreamAnalysisRequest(string analysisText);
    HttpClient BuildHttpClient();
    string RetrieveOpenAiEndpoint();
}
