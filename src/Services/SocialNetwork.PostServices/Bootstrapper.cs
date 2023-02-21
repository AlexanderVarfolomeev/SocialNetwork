using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.PostServices;

public static class Bootstrapper
{
    public static IServiceCollection AddPostServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        return services;
    }
}