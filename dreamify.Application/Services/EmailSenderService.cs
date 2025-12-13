using System.Text;
using dreamify.Application.Abstracts;
using dreamify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using dreamify.Domain.Utils;
using Newtonsoft.Json;

namespace dreamify.Application.Services;

public class EmailSenderService:IEmailSenderService
{
    //private readonly HttpClient _httpClient;
    //private readonly IConfiguration _configuration;

    private readonly UserManager<User> _userManager;
    private readonly IEmailServiceProcessors _emailServiceProcessors;






    public EmailSenderService( IEmailServiceProcessors emailServiceProcessors,UserManager<User> userManager)
    {
         _emailServiceProcessors = emailServiceProcessors;
         _userManager = userManager; 
        
    }

    public async Task SendWelcomeEmail(User user)
    {
        HttpClient EmailSenderClient = _emailServiceProcessors.BuildHttpClient();

        // Generate 6-digit code
        var code = new Random().Next(100000, 999999).ToString();
    
        // Store in user entity
        user.EmailVerificationCode = code;
        user.EmailVerificationCodeExpiry = DateTime.UtcNow.AddMinutes(10);
        await _userManager.UpdateAsync(user);

        var request = new HttpRequestMessage(HttpMethod.Post, "/v3.1/send");
        var emailBody = new
        {
            Messages = new[]
            {
                new {
                    From = new { Email = "support@dream-if-y.us" },
                    To = new[] { new { Email = user.Email } },
                    Subject = "Dreamify Account Confirmation",
                    TextPart = $"Your verification code is: {code}\n\nThis code expires in 10 minutes.",
                    HTMLPart = EmailTemplate.GetConfirmationCodeHtmlPart(user.UserName, code)
                }
            }
        };
    
        request.Content = new StringContent(JsonConvert.SerializeObject(emailBody), Encoding.UTF8, "application/json");
    
        await EmailSenderClient.SendAsync(request);
    }



}