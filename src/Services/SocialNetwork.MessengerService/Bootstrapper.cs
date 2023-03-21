using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.MessengerService;

public static class Bootstrapper
{
    public static IServiceCollection AddMessengerService(this IServiceCollection services)
    {
        services.AddScoped<IMessengerService, MessengerService>();
        return services;
    }
}