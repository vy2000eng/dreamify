namespace dreamify.Infrastructure.Options;

public class MailOptions
{
    public const string MailOptionsKey = "MailOptions";
    public string SecretKey { get; set; }
    public string ApiKey { get; set; }
    public string ApiEndPoint { get; set; }


}