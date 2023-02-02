using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.IdentityServer.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbSettings settings)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseNpgsql(settings.ConnectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseAppDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}