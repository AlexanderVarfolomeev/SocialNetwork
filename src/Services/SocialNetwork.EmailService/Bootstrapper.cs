using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.EmailService;

public static class Bootstrapper
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, EmailService>();
        return services;
    }
}