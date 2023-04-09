using SocialNetwork.EmailService;
using SocialNetwork.RabbitMq;
using SocialNetwork.Settings;
using SocialNetwork.Worker.TaskExecutor;

namespace SocialNetwork.Worker;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddSettings()
            .AddRabbitMq()            
            .AddEmailService()
            ;

        services.AddSingleton<ITaskExecutor, TaskExecutor.TaskExecutor>();

        return services;
    }
}


