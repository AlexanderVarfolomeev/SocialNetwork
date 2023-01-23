using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.WebAPI.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IAppSettings settings)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseNpgsql(settings.Db.GetConnectionString);
        });
        
        return services;
    }
    
}