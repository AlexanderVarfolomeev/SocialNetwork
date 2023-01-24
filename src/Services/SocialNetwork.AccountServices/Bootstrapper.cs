using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.AccountServices;

public static class Bootstrapper
{
    public static IServiceCollection AddAccountService(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        return services;
    }
}