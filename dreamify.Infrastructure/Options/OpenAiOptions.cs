namespace dreamify.Infrastructure.Options;

public class OpenAiOptions
{
    public const string OpenApiOptionsKey = "OpenAiOptions";
    
    public string OpenApiKey        { get; set; }
    public string SystemMessage { get; set; }
    public string Format        { get; set; }
    public float Temp             { get; set; }
    public int MaxTokens        { get; set; }
    public float TopP             { get; set; }
    public string Model         { get; set; }
    public string UserRole      { get; set; }
    public string SystemRole    { get; set; }
    public string Authorization {get; set;} 
    public string Bearer        {get; set;}
    public string Uri           {get; set;}
    public string ApiEndpoint   {get; set;}


}