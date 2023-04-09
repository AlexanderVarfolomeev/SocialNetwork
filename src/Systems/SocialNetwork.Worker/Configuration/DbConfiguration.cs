using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Context.Setup;
using SocialNetwork.Settings.Interfaces;

namespace SocialNetwork.Worker.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IAppSettings settings)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseNpgsql(settings.Db.ConnectionString);
        });
        
        return services;
    }
}