using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.AttachmentServices;

public static class Bootstrapper
{
    public static IServiceCollection AddAttachmentService(this IServiceCollection services)
    {
        services.AddScoped<IAttachmentService, AttachmentService>();
        return services;
    }
}