using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Settings.Settings;

namespace SocialNetwork.RabbitMq;

public static class Bootstrapper
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMq, RabbitMq>();

        return services;
    }
}