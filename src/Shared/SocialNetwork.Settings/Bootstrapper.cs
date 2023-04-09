using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Settings.Interfaces;
using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;

namespace SocialNetwork.Settings;

public static class Bootstrapper
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<ISettingSource, SettingSource>();
        services.AddSingleton<IAppSettings,AppSettings>();
        services.AddSingleton<IEmailSettings, EmailSettings>();
        services.AddSingleton<IDbSettings, DbSettings>();
        services.AddSingleton<IIdentitySettings, IdentitySettings>();
        services.AddSingleton<IRedisSettings, RedisSettings>();
        services.AddSingleton<IRabbitMqSettings, RabbitMqSettings>();
        return services;
    }
    
}