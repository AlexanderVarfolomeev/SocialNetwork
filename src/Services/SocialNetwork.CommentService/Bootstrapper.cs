using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.CommentService;

public static class Bootstrapper
{
    public static IServiceCollection AddCommentService(this IServiceCollection services)
    {
        services.AddScoped<ICommentService, CommentService>();
        return services;
    }
}