using System.Text;
using System.Text.Json;
using dreamify.Application.Abstracts;
using dreamify.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace dreamify.Infrastructure.Processors;

public class OpenAiRequestProcessor:IOpenAiRequestProcessor
{
    private readonly OpenAiOptions _openAiOptions;
    //private readonly HttpClient _client;


    public OpenAiRequestProcessor(IOptions<OpenAiOptions> openAiOptions)
    {
        _openAiOptions = openAiOptions.Value;
    }

    public HttpClient BuildHttpClient()
    {
        HttpClient openaiClient = new HttpClient();
        openaiClient.BaseAddress = new Uri(_openAiOptions.Uri);

        openaiClient.DefaultRequestHeaders.Add(_openAiOptions.Authorization, $"{_openAiOptions.Bearer} {_openAiOptions.OpenApiKey}");
        return openaiClient;
        


    }

    public string RetrieveOpenAiEndpoint()
    {
        return _openAiOptions.ApiEndpoint;

    }

    public StringContent GenerateDreamAnalysisRequest(string analysisText)
    {
        var requestBody = new
        {
            model = _openAiOptions.Model,
            messages = new[]
            {
                new
                {
                    role = _openAiOptions.SystemRole,
                    content = new[]
                    {
                        new
                        {
                            type = _openAiOptions.Format,
                            text = _openAiOptions.SystemMessage
                        }
                    }
                },
                new
                {
                    role = _openAiOptions.UserRole,
                    content = new[]
                    {
                        new
                        {
                            type = _openAiOptions.Format,
                            text = analysisText
                        }
                    }
                }
            },
            temperature = _openAiOptions.Temp,
            max_tokens = _openAiOptions.MaxTokens,
            top_p = _openAiOptions.TopP,
            frequency_penalty = 0,
            presence_penalty = 0,
            response_format = new
            {
                type = _openAiOptions.Format,
            }
        };
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }







}