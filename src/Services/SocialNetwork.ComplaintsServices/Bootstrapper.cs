using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.ComplaintsServices;

public static class Bootstrapper
{
    public static IServiceCollection AddComplaintService(this IServiceCollection services)
    {
        services.AddScoped<IComplaintService, ComplaintService>();
        return services;
    }
}