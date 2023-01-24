using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Common.Helpers;

public static class AutoMapperRegisterService
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("socialnetwork."));

        services.AddAutoMapper(assemblies);
    }
}