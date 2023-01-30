using SocialNetwork.EmailService.Models;

namespace SocialNetwork.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(EmailModel model);
}