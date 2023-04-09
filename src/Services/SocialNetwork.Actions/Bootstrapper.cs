using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Actions;

public static class Bootstrapper
{
    public static IServiceCollection AddActions(this IServiceCollection services)
    {
        services.AddSingleton<IAction, Action>();

        return services;
    }
}