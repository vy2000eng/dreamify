using System.Text;
using System.Text.Json;
using dreamify.Application.Abstracts;
using dreamify.Domain.Exceptions;
using dreamify.Domain.Requests;
using dreamify.Domain.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace dreamify.Application.Services;

public class OpenAiService:IOpenAiService
{
    private readonly IOpenAiRequestProcessor _openAiRequestProcessor;
    
    public OpenAiService(IConfiguration configuration, IOpenAiRequestProcessor openAiRequestProcessor)
    {
        _openAiRequestProcessor = openAiRequestProcessor;

    }

    public async Task<AnalysisResponse> AnayzeDream(AnalysisRequest request)
    {
        HttpClient openAiclient = _openAiRequestProcessor.BuildHttpClient();
        var content = _openAiRequestProcessor.GenerateDreamAnalysisRequest(request.TextToAnalyze);
        var endpoint = _openAiRequestProcessor.RetrieveOpenAiEndpoint();

        try
        {

            var response = await openAiclient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseString);
            var analysis = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response content found";

            return new AnalysisResponse
            {
                DreamAnalysisResponse = analysis,

            };

        }
        catch (HttpRequestException ex)
        {
            throw new OpenAiAnalysisFiled(ex.HttpRequestError, ex.Message);
        }
    }
}