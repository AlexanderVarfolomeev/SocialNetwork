using SocialNetwork.EmailService.Models;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.EmailService;

public class EmailService : IEmailService
{
    private readonly IAppSettings _settings;

    public EmailService(IAppSettings settings)
    {
        _settings = settings;
    }
    public async Task SendEmailAsync(EmailModel model)
    {
        await new SmtpProvider(_settings.Email).SendEmailAsync(model.Email, model.Subject, model.Message);
    }
}