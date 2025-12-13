using dreamify.Domain.Entities;

namespace dreamify.Application.Abstracts;

public interface IEmailSenderService
{
    Task SendWelcomeEmail(User user);


}