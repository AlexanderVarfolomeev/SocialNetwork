using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Entities.Files;

namespace SocialNetwork.AttachmentServices;

public static class Bootstrapper
{
    public static IServiceCollection AddAttachmentService(this IServiceCollection services)
    {
        services.AddScoped<IAttachmentService, AttachmentService>();
        return services;
    }
}