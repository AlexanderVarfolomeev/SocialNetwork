using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Repository;

public static class Bootstrapper
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
        return services;
    }
}