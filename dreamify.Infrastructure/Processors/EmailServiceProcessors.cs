using System.Net.Http.Headers;
using System.Text;
using dreamify.Application.Abstracts;
using dreamify.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace dreamify.Infrastructure.Processors;

public class EmailServiceProcessors:IEmailServiceProcessors
{
    private readonly MailOptions _mailOptions;
    private readonly string _credentials;

    public EmailServiceProcessors(IOptions<MailOptions> mailOptions)
    {
        _mailOptions = mailOptions.Value;
        _credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_mailOptions.ApiKey}:{_mailOptions.SecretKey}"));

    }
    
    public HttpClient BuildHttpClient()
    {
        HttpClient _mailClient = new HttpClient();
        _mailClient.BaseAddress = new Uri(_mailOptions.ApiEndPoint)
            ;
        _mailClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

        return _mailClient;



    }
    public string RetrieveOpenAiEndpoint()
    {
        return _mailOptions.ApiEndPoint;

    }

    
}