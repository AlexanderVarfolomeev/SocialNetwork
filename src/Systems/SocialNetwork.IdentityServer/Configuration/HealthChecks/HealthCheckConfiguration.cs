using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SocialNetwork.Common.HealthChecks;

namespace SocialNetwork.IdentityServer.Configuration.HealthChecks;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<SelfHealthCheck>("SocialNetwork.IdentityServer");

        return services;
    }

    public static void UseAppHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapHealthChecks("/health/detail", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckHelper.WriteHealthCheckResponse,
            AllowCachingResponses = false,
        });
    }
}