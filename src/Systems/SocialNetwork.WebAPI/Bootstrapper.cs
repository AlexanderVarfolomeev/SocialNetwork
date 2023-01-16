using SocialNetwork.Settings;

namespace SocialNetwork.WebAPI;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSettings();
        return services;
    }
}