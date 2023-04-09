using SocialNetwork.Constants.RabbitMq;
using SocialNetwork.EmailService.Models;
using SocialNetwork.RabbitMq;

namespace SocialNetwork.Actions;

public class Action : IAction
{
    private readonly IRabbitMq _rabbitMq;

    public Action(IRabbitMq rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }
    public async Task SendEmail(EmailModel model)
    {
        await _rabbitMq.PushAsync(ActionQueueName.SendEmail, model);
    }
}