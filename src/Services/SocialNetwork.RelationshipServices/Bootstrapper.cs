using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.RelationshipServices;

public static class Bootstrapper
{
    public static IServiceCollection AddRelationshipService(this IServiceCollection services)
    {
        services.AddScoped<IRelationshipService, RelationshipService>();
        return services;
    }
}