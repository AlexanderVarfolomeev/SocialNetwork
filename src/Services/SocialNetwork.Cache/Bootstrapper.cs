using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Cache;

public static class Bootstrapper
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}