using SocialNetwork.Constants.RabbitMq;
using SocialNetwork.EmailService;
using SocialNetwork.EmailService.Models;
using SocialNetwork.RabbitMq;

namespace SocialNetwork.Worker.TaskExecutor;

public class TaskExecutor : ITaskExecutor
{
    private readonly ILogger<TaskExecutor> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IRabbitMq rabbitMq;

    public TaskExecutor(
        ILogger<TaskExecutor> logger,
        IServiceProvider serviceProvider,
        IRabbitMq rabbitMq
    )
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.rabbitMq = rabbitMq;
    }

    private async Task Execute<T>(Func<T, Task> action)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<T>();
            if (service != null)
                await action(service);
            else
                logger.LogError($"Error: {action.ToString()} wasn`t resolved");
        }
        catch (Exception e)
        {
            logger.LogError($"Error: {ActionQueueName.SendEmail}: {e.Message}");
            throw;
        }
    }

    public void Start()
    {        
        rabbitMq.Subscribe<EmailModel>(ActionQueueName.SendEmail, async data
            => await Execute<IEmailService>(async service =>
            {
                logger.LogDebug($"RABBITMQ::: {ActionQueueName.SendEmail}: {data.Email} {data.Message}");
                await service.SendEmailAsync(data);
            }));
    }
}