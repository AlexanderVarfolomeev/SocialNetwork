using MailKit.Net.Smtp;
using MimeKit;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.EmailService;

public class SmtpProvider
{
    private readonly IEmailSettings _settings;

    public SmtpProvider(IEmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        try
        {
            client.ServerCertificateValidationCallback = (_, _, _, _) => true;
            await client.ConnectAsync(_settings.Server, _settings.Port, _settings.Ssl);
            await client.AuthenticateAsync(_settings.Login, _settings.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}