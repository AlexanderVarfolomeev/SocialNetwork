using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.GroupServices;

public static class Bootstrapper
{
    public static IServiceCollection AddGroupService(this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        return services;
    }
}