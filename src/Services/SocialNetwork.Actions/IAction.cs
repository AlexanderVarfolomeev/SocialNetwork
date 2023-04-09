using SocialNetwork.EmailService.Models;

namespace SocialNetwork.Actions;

public interface IAction
{
    Task SendEmail(EmailModel model);
}