using SocialNetwork.Common.Helpers;

namespace SocialNetwork.WebAPI.Configuration;

public static class AutomapperConfiguration
{
    public static IServiceCollection AddAppAutomapper(this IServiceCollection services)
    {
        AutoMapperRegisterService.Register(services);
        return services;
    }
}