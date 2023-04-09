using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.RabbitMq;

public static class Bootstrapper
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMq, RabbitMq>();

        return services;
    }
}