using SocialNetwork.AccountServices;
using SocialNetwork.Repository;
using SocialNetwork.Settings;

namespace SocialNetwork.WebAPI;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddRepository();
        services.AddAccountService();
        return services;
    }
}